using pg.im.model.data;
using System;
using UniRx;
using Zenject;

namespace pg.im.model.remote
{
    public class WarehouseRemoteDataModel
    {
        [Inject] private readonly StaticDataModel _staticDataModel;
        [Inject] private readonly RemoteDataModel _remoteDataModel;

        private CompositeDisposable _disposables;

        public ReactiveProperty<WarehouseRemoteData> ReactiveWarehouse;

        public WarehouseRemoteData WarehouseRemoteData
        {
            get
            {
                return ReactiveWarehouse.Value;
            }
            set
            {
                ReactiveWarehouse.SetValueAndForceNotify(value);
            }
        }

        public WarehouseRemoteDataModel()
        {
            ReactiveWarehouse = new ReactiveProperty<WarehouseRemoteData>();
            _disposables = new CompositeDisposable();
        }

        public void SeedWarehouseRemoteData(WarehouseRemoteData warehouseRemoteData)
        {
            this.WarehouseRemoteData = warehouseRemoteData;

            if (warehouseRemoteData.WarehouseLevelData == null)
            {
                warehouseRemoteData.WarehouseLevelData = _staticDataModel.GetWarehouseLevelData(warehouseRemoteData.WarehouseLevel);
            }

            Observable.Timer(TimeSpan.FromSeconds(1)).Repeat().Subscribe((interval) => Tick()).AddTo(_disposables);
        }

        public void AddCash(double cash)
        {
            _remoteDataModel.UpdateCash(_remoteDataModel.Cash.Value + cash);
        }

        private void Tick()
        {
            if (!(WarehouseRemoteData == null || WarehouseRemoteData.WarehouseLevelData == null))
            {
                foreach (TransporterRemoteData transporter in WarehouseRemoteData.Transporters)
                {
                    TransporterTick(transporter);
                }

                ReactiveWarehouse.SetValueAndForceNotify(WarehouseRemoteData);
            }
        }

        private void TransporterTick(TransporterRemoteData transporter)
        {
            switch (transporter.TransporterState)
            {
                case ETransporterState.Idle:
                    if (!string.IsNullOrEmpty(WarehouseRemoteData.Manager))
                    {
                        transporter.TransporterState = ETransporterState.WalkingToElevator;
                    }
                    break;
                case ETransporterState.WalkingToElevator:
                    if ((transporter.CurrentLocation += (int)WarehouseRemoteData.WarehouseLevelData.WalkSpeed) >= _staticDataModel.MetaData.WarehouseDistance)
                    {
                        transporter.TransporterState = ETransporterState.Loading;
                    }
                    break;
                case ETransporterState.WalkingToWarehouse:
                    if ((transporter.CurrentLocation -= (int)WarehouseRemoteData.WarehouseLevelData.WalkSpeed) <= 0)
                    {
                        AddCash(transporter.LoadedCash);
                        transporter.LoadedCash = 0;
                        transporter.TransporterState = WarehouseRemoteData.Manager == null ? ETransporterState.Idle : ETransporterState.WalkingToElevator;
                    }
                    break;
                case ETransporterState.Loading:
                    double cashToLoad = WarehouseRemoteData.WarehouseLevelData.LoadingSpeed;
                    if (transporter.LoadedCash + cashToLoad > WarehouseRemoteData.WarehouseLevelData.LoadPerTransporter)
                        cashToLoad = WarehouseRemoteData.WarehouseLevelData.LoadPerTransporter - transporter.LoadedCash;
                    double cashLoaded = _remoteDataModel.Elevator.RemoveStoredCash(cashToLoad);

                    transporter.LoadedCash += cashLoaded;

                    if (cashToLoad != cashLoaded || transporter.LoadedCash == WarehouseRemoteData.WarehouseLevelData.LoadPerTransporter)
                        transporter.TransporterState = ETransporterState.WalkingToWarehouse;
                    break;
            }

            //UnityEngine.Debug.LogError(string.Format("State: {0}, Location: {1}, Loaded: {2}", transporter.TransporterState.ToString(), transporter.CurrentLocation, transporter.LoadedCash));
        }

        public class Factory : Factory<WarehouseRemoteDataModel>
        {
        }
    }
}