using System;
using PG.IdleMiner.Misc;
using PG.IdleMiner.Models.DataModels;
using UniRx;
using UnityEngine;
using Zenject;

namespace PG.IdleMiner.Models.RemoteDataModels
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
            WarehouseRemoteData = warehouseRemoteData;

            if (warehouseRemoteData.WarehouseLevelData == null)
            {
                warehouseRemoteData.WarehouseLevelData = _staticDataModel.GetWarehouseLevelData(warehouseRemoteData.WarehouseLevel);
            }

            // TODO: Move this to respective Facade.
            Observable.EveryFixedUpdate().Subscribe((interval) => Tick()).AddTo(_disposables);
        }

        public void Upgrade()
        {
            if (WarehouseRemoteData.WarehouseLevel < _staticDataModel.MetaData.Warehouse.Count)
            {
                WarehouseRemoteData.WarehouseLevel++;
                WarehouseRemoteData.WarehouseLevelData = _staticDataModel.GetWarehouseLevelData(WarehouseRemoteData.WarehouseLevel);
            }
        }
        
        public void AddCash(double cash)
        {
            _remoteDataModel.UpdateCash(_remoteDataModel.Cash.Value + cash);
        }

        float _previousStamp = float.MinValue;
        private void Tick()
        {
            if (WarehouseRemoteData?.WarehouseLevelData == null) return;
            
            while (WarehouseRemoteData.Transporters.Count < WarehouseRemoteData.WarehouseLevelData.Transporters)
            {
                WarehouseRemoteData.Transporters.Add(new TransporterRemoteData());
            }
            
            if (Math.Abs(_previousStamp - float.MinValue) < 0.0001f)
            {
                _previousStamp = Time.time;
                return;
            }

            float interval = (Time.time - _previousStamp);
            _previousStamp = Time.time;
                
            foreach (TransporterRemoteData transporter in WarehouseRemoteData.Transporters)
            {
                TransporterTick(transporter, interval);
            }

            ReactiveWarehouse.SetValueAndForceNotify(WarehouseRemoteData);
        }

        // TODO: Not the best place to do this. Add respective Facades and then do this there. 
        // For now doing it here.
        private void TransporterTick(TransporterRemoteData transporter, float interval)
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
                    if ((transporter.CurrentLocation += WarehouseRemoteData.WarehouseLevelData.WalkSpeed * interval) >= _staticDataModel.MetaData.WarehouseDistance)
                    {
                        transporter.TransporterState = ETransporterState.Loading;
                    }
                    break;
                case ETransporterState.WalkingToWarehouse:
                    if ((transporter.CurrentLocation -= (int)WarehouseRemoteData.WarehouseLevelData.WalkSpeed * interval) <= 0)
                    {
                        AddCash(transporter.LoadedCash);
                        transporter.LoadedCash = 0;
                        transporter.TransporterState = WarehouseRemoteData.Manager == null ? ETransporterState.Idle : ETransporterState.WalkingToElevator;
                    }
                    break;
                case ETransporterState.Loading:
                    double cashToLoad = WarehouseRemoteData.WarehouseLevelData.LoadingSpeed * interval;
                    if (transporter.LoadedCash + cashToLoad > WarehouseRemoteData.WarehouseLevelData.LoadPerTransporter)
                        cashToLoad = WarehouseRemoteData.WarehouseLevelData.LoadPerTransporter - transporter.LoadedCash;
                    double cashLoaded = _remoteDataModel.Elevator.RemoveStoredCash(cashToLoad);

                    transporter.LoadedCash += cashLoaded;

                    if (Math.Abs(cashToLoad - cashLoaded) > 0.1f || transporter.LoadedCash >= WarehouseRemoteData.WarehouseLevelData.LoadPerTransporter)
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