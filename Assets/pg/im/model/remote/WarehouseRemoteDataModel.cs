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

        public WarehouseRemoteData _warehouseRemoteData;

        public WarehouseRemoteDataModel()
        {
            _disposables = new CompositeDisposable();
        }

        public void SeedWarehouseRemoteData(WarehouseRemoteData warehouseRemoteData)
        {
            this._warehouseRemoteData = warehouseRemoteData;

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
            if (!(_warehouseRemoteData == null || _warehouseRemoteData.WarehouseLevelData == null))
            {
                foreach (TransporterRemoteData transporter in _warehouseRemoteData.Transporters)
                {
                    TransporterTick(transporter);
                }
            }
        }

        private void TransporterTick(TransporterRemoteData transporter)
        {
            switch (transporter.TransporterState)
            {
                case ETransporterState.Idle:
                    if (!string.IsNullOrEmpty(_warehouseRemoteData.Manager))
                    {
                        transporter.TransporterState = ETransporterState.WalkingToElevator;
                    }
                    break;
                case ETransporterState.WalkingToElevator:
                    if ((transporter.CurrentLocation += (int)_warehouseRemoteData.WarehouseLevelData.WalkSpeed) >= _staticDataModel.MetaData.WarehouseDistance)
                    {
                        transporter.TransporterState = ETransporterState.Loading;
                    }
                    break;
                case ETransporterState.WalkingToWarehouse:
                    if ((transporter.CurrentLocation -= (int)_warehouseRemoteData.WarehouseLevelData.WalkSpeed) <= 0)
                    {
                        AddCash(transporter.LoadedCash);
                        transporter.LoadedCash = 0;
                        transporter.TransporterState = _warehouseRemoteData.Manager == null ? ETransporterState.Idle : ETransporterState.WalkingToElevator;
                    }
                    break;
                case ETransporterState.Loading:
                    double cashToLoad = _warehouseRemoteData.WarehouseLevelData.LoadingSpeed;
                    if (transporter.LoadedCash + cashToLoad > _warehouseRemoteData.WarehouseLevelData.LoadPerTransporter)
                        cashToLoad = _warehouseRemoteData.WarehouseLevelData.LoadPerTransporter - transporter.LoadedCash;
                    double cashLoaded = _remoteDataModel.Elevator.RemoveStoredCash(cashToLoad);

                    transporter.LoadedCash += cashLoaded;

                    if (cashToLoad != cashLoaded || transporter.LoadedCash == _warehouseRemoteData.WarehouseLevelData.LoadPerTransporter)
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