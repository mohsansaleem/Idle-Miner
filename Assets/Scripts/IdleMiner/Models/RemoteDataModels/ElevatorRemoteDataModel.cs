using System;
using PG.IdleMiner.Misc;
using PG.IdleMiner.Models.DataModels;
using UniRx;
using UnityEngine;
using Zenject;

namespace PG.IdleMiner.Models.RemoteDataModels
{
    public class ElevatorRemoteDataModel
    {
        [Inject] private readonly StaticDataModel _staticDataModel;
        [Inject] private readonly RemoteDataModel _remoteDataModel;

        private CompositeDisposable _disposables;
        public ReactiveProperty<ElevatorRemoteData> ReactiveElevator;

        public ElevatorRemoteData ElevatorRemoteData
        {
            get
            {
                return ReactiveElevator.Value;
            }
            set
            {
                ReactiveElevator.SetValueAndForceNotify(value);
            }
        }

        public ElevatorRemoteDataModel()
        {
            ReactiveElevator = new ReactiveProperty<ElevatorRemoteData>();
            _disposables = new CompositeDisposable();
        }

        public void SeedElevatorRemoteData(ElevatorRemoteData elevatorRemoteData)
        {
            _disposables.Dispose();
            _disposables = new CompositeDisposable();

            ElevatorRemoteData = elevatorRemoteData;

            if (elevatorRemoteData.ElevatorLevelData == null)
            {
                elevatorRemoteData.ElevatorLevelData = _staticDataModel.GetElevatorLevelData(elevatorRemoteData.ElevatorLevel);
            }

            // TODO: Move this to respective Facade.
            Observable.EveryFixedUpdate().Subscribe((interval) => Tick()).AddTo(_disposables);
        }

        public void Upgrade()
        {
            if (ElevatorRemoteData.ElevatorLevel < _staticDataModel.MetaData.Elevator.Count)
            {
                ElevatorRemoteData.ElevatorLevel++;
                ElevatorRemoteData.ElevatorLevelData = _staticDataModel.GetElevatorLevelData(ElevatorRemoteData.ElevatorLevel);
            }
        }

        public void SetLoadedCash(double cash)
        {
            ElevatorRemoteData.LoadedCash = cash;
        }

        public void SetStoredCash(double cash)
        {
            ElevatorRemoteData.StoredCash = cash;
        }

        public double RemoveStoredCash(double cash)
        {
            if (cash > ElevatorRemoteData.StoredCash)
            {
                cash = ElevatorRemoteData.StoredCash;
            }

            SetStoredCash(ElevatorRemoteData.StoredCash - cash);

            return cash;
        }

        private int MaxDistance
        {
            get
            {
                return _remoteDataModel.Shafts.Count * _staticDataModel.MetaData.ShaftDepth;
            }
        }

        // TODO: Not the best place to do this. Add respective Facades and then do this there. 
        // For now doing it here.
        private void Tick()
        {
            if (!(ElevatorRemoteData == null || ElevatorRemoteData.ElevatorLevelData == null))
            {
                int shaftNumber;
                float interval = Time.fixedDeltaTime;

                ShaftRemoteDataModel shaft;

                switch (ElevatorRemoteData.ElevatorState)
                {
                    case EElevatorState.Idle:
                        if (!string.IsNullOrEmpty(ElevatorRemoteData.Manager))
                        {
                            ElevatorRemoteData.ElevatorState = EElevatorState.MovingDown;
                        }
                        break;
                    case EElevatorState.MovingDown:
                        if ((ElevatorRemoteData.CurrentLocation + (ElevatorRemoteData.ElevatorLevelData.MovementSpeed * interval)) > MaxDistance)
                        {
                            ElevatorRemoteData.CurrentLocation = MaxDistance;
                        }
                        else
                        {
                            ElevatorRemoteData.CurrentLocation += (ElevatorRemoteData.ElevatorLevelData.MovementSpeed * interval);
                        }

                        if (ElevatorRemoteData.CurrentLocation % _staticDataModel.MetaData.ShaftDepth <= 0.1f && 
                            ElevatorRemoteData.CurrentLocation >= _staticDataModel.MetaData.ShaftDepth)
                        {
                            shaftNumber = (int)(ElevatorRemoteData.CurrentLocation / _staticDataModel.MetaData.ShaftDepth);

                            if (shaftNumber < 1)
                                shaftNumber = 1;

                            shaft = _remoteDataModel.Shafts[shaftNumber - 1];

                            if (shaft.ShaftRemoteData.BinCash < 1)
                            {
                                ElevatorRemoteData.ElevatorState = ElevatorRemoteData.CurrentLocation >= MaxDistance ? EElevatorState.MovingUp : EElevatorState.MovingDown;
                            }
                            else
                                ElevatorRemoteData.ElevatorState = EElevatorState.Loading;
                        }
                        break;
                    case EElevatorState.MovingUp:
                        if ((ElevatorRemoteData.CurrentLocation -= ElevatorRemoteData.ElevatorLevelData.MovementSpeed * interval) <= 0)
                        {
                            SetStoredCash(ElevatorRemoteData.StoredCash + ElevatorRemoteData.LoadedCash);
                            ElevatorRemoteData.LoadedCash = 0;
                            ElevatorRemoteData.ElevatorState = ElevatorRemoteData.Manager == null ? EElevatorState.Idle : EElevatorState.MovingDown;
                        }
                        break;
                    case EElevatorState.Loading:

                        shaftNumber = (int)(ElevatorRemoteData.CurrentLocation / _staticDataModel.MetaData.ShaftDepth);

                        if (shaftNumber < 1)
                            shaftNumber = 1;
                        
                        shaft = _remoteDataModel.Shafts[(shaftNumber  - 1) % _remoteDataModel.Shafts.Count];

                        double cashToLoad = ElevatorRemoteData.ElevatorLevelData.LoadingSpeed * interval;
                        if (ElevatorRemoteData.LoadedCash + cashToLoad > ElevatorRemoteData.ElevatorLevelData.LoadCapacity)
                            cashToLoad = ElevatorRemoteData.ElevatorLevelData.LoadCapacity - ElevatorRemoteData.LoadedCash;
                        double cashLoaded = shaft.RemoveBinCash(cashToLoad);

                        SetLoadedCash(ElevatorRemoteData.LoadedCash + cashLoaded);

                        // If Bin Empty
                        if (shaft.ShaftRemoteData.BinCash < 1)
                        {
                            ElevatorRemoteData.ElevatorState = ElevatorRemoteData.CurrentLocation >= MaxDistance ? EElevatorState.MovingUp : EElevatorState.MovingDown;
                        }

                        if (Math.Abs(ElevatorRemoteData.LoadedCash - ElevatorRemoteData.ElevatorLevelData.LoadCapacity) < 0.1f)
                        {
                            ElevatorRemoteData.ElevatorState = EElevatorState.MovingUp;
                        }
                        break;
                }

                ReactiveElevator.SetValueAndForceNotify(ElevatorRemoteData);
            }
        }

        public class Factory : Factory<ElevatorRemoteDataModel>
        {
        }
    }
}