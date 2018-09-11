using pg.im.model.data;
using System;
using UniRx;
using Zenject;

namespace pg.im.model.remote
{
    public class ShaftRemoteDataModel
    {
        [Inject] private readonly StaticDataModel _staticDataModel;

        private CompositeDisposable _disposables;

        public ShaftRemoteData _shaftRemoteData;

        public ReactiveProperty<double> BinCash;

        public ShaftRemoteDataModel()
        {
            BinCash = new ReactiveProperty<double>(0);
            _disposables = new CompositeDisposable();
        }

        public void SeedShaftRemoteData(ShaftRemoteData shaftRemoteData)
        {
            this._shaftRemoteData = shaftRemoteData;

            if (shaftRemoteData.ShaftLevelData == null)
            {
                shaftRemoteData.ShaftLevelData = _staticDataModel.GetShaftLevelData(shaftRemoteData.ShaftId,
                                                                                    shaftRemoteData.ShaftLevel);
            }

            BinCash.Value = shaftRemoteData.BinCash;

            Observable.Timer(TimeSpan.FromSeconds(1)).Repeat().Subscribe((interval) => Tick()).AddTo(_disposables);
        }

        public void SetBinCash(double cash)
        {
            _shaftRemoteData.BinCash = cash;
            BinCash.Value = cash;
        }

        public double RemoveBinCash(double cash)
        {
            if (cash > _shaftRemoteData.BinCash)
            {
                cash = _shaftRemoteData.BinCash;
            }

            SetBinCash(_shaftRemoteData.BinCash - cash);

            return cash;
        }

        private void Tick()
        {
            if (!(_shaftRemoteData == null || _shaftRemoteData.ShaftLevelData == null))
            {
                foreach (MinerRemoteData miner in _shaftRemoteData.Miners)
                {
                    MinerTick(miner);
                }
            }
        }

        private void MinerTick(MinerRemoteData miner)
        {
            switch (miner.MinerState)
            {
                case EMinerState.Idle:
                    if (!string.IsNullOrEmpty(_shaftRemoteData.Manager))
                    {
                        miner.MinerState = EMinerState.WalkingToMine;
                    }
                    break;
                case EMinerState.WalkingToMine:
                    if ((miner.CurrentLocation += (int)_shaftRemoteData.ShaftLevelData.WalkSpeed) >= _staticDataModel.MetaData.MineLength)
                    { 
                        miner.MinerState = EMinerState.Mining;
                    }
                    break;
                case EMinerState.WalkingToBin:
                    if ((miner.CurrentLocation -= (int)_shaftRemoteData.ShaftLevelData.WalkSpeed) <= 0)
                    {
                        SetBinCash(_shaftRemoteData.BinCash + miner.MinedCash);
                        miner.MinedCash = 0;
                        miner.MinerState = _shaftRemoteData.Manager == null ? EMinerState.Idle : EMinerState.WalkingToMine;
                    }
                    break;
                case EMinerState.Mining:
                    miner.MinedCash += _shaftRemoteData.ShaftLevelData.MinningSpeed;
                    if(miner.MinedCash >= _shaftRemoteData.ShaftLevelData.WorkerCapacity)
                    {
                        miner.MinedCash = _shaftRemoteData.ShaftLevelData.WorkerCapacity;
                        miner.MinerState = EMinerState.WalkingToBin;
                    }
                    break;
            }
        }

        public class Factory : Factory<ShaftRemoteDataModel>
        {
        }
    }
}