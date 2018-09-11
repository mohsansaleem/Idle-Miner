using pg.im.model.data;
using System;
using UniRx;
using Zenject;

namespace pg.im.model.remote
{
    public class ElevatorRemoteDataModel
    {
        [Inject] private readonly StaticDataModel _staticDataModel;
        [Inject] private readonly RemoteDataModel _remoteDataModel;

        private CompositeDisposable _disposables;

        public ElevatorRemoteData _elevatorRemoteData;

        public ReactiveProperty<double> StoredCash;
        public ReactiveProperty<double> LoadedCash;

        public ElevatorRemoteDataModel()
        {
            StoredCash = new ReactiveProperty<double>(0);
            LoadedCash = new ReactiveProperty<double>(0);
            _disposables = new CompositeDisposable();
        }

        public void SeedElevatorRemoteData(ElevatorRemoteData elevatorRemoteData)
        {
            this._elevatorRemoteData = elevatorRemoteData;

            if (elevatorRemoteData.ElevatorLevelData == null)
            {
                elevatorRemoteData.ElevatorLevelData = _staticDataModel.GetElevatorLevelData(elevatorRemoteData.ElevatorLevel);
            }

            StoredCash.Value = elevatorRemoteData.StoredCash;
            LoadedCash.Value = elevatorRemoteData.LoadedCash;

            Observable.Timer(TimeSpan.FromSeconds(1)).Repeat().Subscribe((interval) => Tick()).AddTo(_disposables);
        }


        public void SetLoadedCash(double cash)
        {
            _elevatorRemoteData.LoadedCash = cash;
            LoadedCash.Value = cash;
        }

        public void SetStoredCash(double cash)
        {
            _elevatorRemoteData.StoredCash = cash;
            StoredCash.Value = cash;
        }

        public double RemoveStoredCash(double cash)
        {
            if (cash > _elevatorRemoteData.StoredCash)
            {
                cash = _elevatorRemoteData.StoredCash;
            }

            SetStoredCash(_elevatorRemoteData.StoredCash - cash);

            return cash;
        }

        private int MaxDistance
        {
            get
            {
                return _remoteDataModel.Shafts.Count * _staticDataModel.MetaData.ShaftDepth;
            }
        }

        private void Tick()
        {
            if (!(_elevatorRemoteData == null || _elevatorRemoteData.ElevatorLevelData == null))
            {
                int shaftNumber = 1;

                ShaftRemoteDataModel shaft;

                switch (_elevatorRemoteData.ElevatorState)
                {
                    case EElevatorState.Idle:
                        if (!string.IsNullOrEmpty(_elevatorRemoteData.Manager))
                        {
                            _elevatorRemoteData.ElevatorState = EElevatorState.MovingDown;
                        }
                        break;
                    case EElevatorState.MovingDown:

                        if (_elevatorRemoteData.CurrentLocation + _elevatorRemoteData.ElevatorLevelData.MovementSpeed > MaxDistance)
                        {
                            _elevatorRemoteData.CurrentLocation = MaxDistance;
                        }
                        else
                        {
                            _elevatorRemoteData.CurrentLocation += _elevatorRemoteData.ElevatorLevelData.MovementSpeed;
                        }

                        if (_elevatorRemoteData.CurrentLocation % _staticDataModel.MetaData.ShaftDepth == 0)
                        {
                            shaftNumber = _elevatorRemoteData.CurrentLocation / _staticDataModel.MetaData.ShaftDepth;

                            shaft = _remoteDataModel.Shafts[shaftNumber - 1];

                            if (shaft.BinCash.Value < 1)
                            {
                                _elevatorRemoteData.ElevatorState = _elevatorRemoteData.CurrentLocation >= MaxDistance ? EElevatorState.MovingUp : EElevatorState.MovingDown;
                            }
                            else
                                _elevatorRemoteData.ElevatorState = EElevatorState.Loading;
                        }
                        break;
                    case EElevatorState.MovingUp:
                        if ((_elevatorRemoteData.CurrentLocation -= _elevatorRemoteData.ElevatorLevelData.MovementSpeed) <= 0)
                        {
                            SetStoredCash(_elevatorRemoteData.StoredCash + _elevatorRemoteData.LoadedCash);
                            _elevatorRemoteData.LoadedCash = 0;
                            _elevatorRemoteData.ElevatorState = _elevatorRemoteData.Manager == null ? EElevatorState.Idle : EElevatorState.MovingDown;
                        }
                        break;
                    case EElevatorState.Loading:

                        shaftNumber = _elevatorRemoteData.CurrentLocation / _staticDataModel.MetaData.ShaftDepth;

                        shaft = _remoteDataModel.Shafts[shaftNumber - 1];

                        double cashToLoad = _elevatorRemoteData.ElevatorLevelData.LoadingSpeed;
                        if (_elevatorRemoteData.LoadedCash + cashToLoad > _elevatorRemoteData.ElevatorLevelData.LoadCapacity)
                            cashToLoad = _elevatorRemoteData.ElevatorLevelData.LoadCapacity - _elevatorRemoteData.LoadedCash;
                        double cashLoaded = shaft.RemoveBinCash(cashToLoad);

                        SetLoadedCash(_elevatorRemoteData.LoadedCash + cashLoaded);

                        // If Bin Empty
                        if (shaft.BinCash.Value < 1)
                        {
                            _elevatorRemoteData.ElevatorState = _elevatorRemoteData.CurrentLocation >= MaxDistance ? EElevatorState.MovingUp : EElevatorState.MovingDown;
                        }
                        break;
                }
            }
        }

        public class Factory : Factory<ElevatorRemoteDataModel>
        {
        }
    }
}