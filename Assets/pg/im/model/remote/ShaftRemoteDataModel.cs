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

        public ReactiveProperty<ShaftRemoteData> ReactiveShaft;

        public ShaftRemoteData ShaftRemoteData
        {
            get
            {
                return ReactiveShaft.Value;
            }
            set
            {
                ReactiveShaft.SetValueAndForceNotify(value);
            }
        }

        public ShaftRemoteDataModel()
        {
            ReactiveShaft = new ReactiveProperty<ShaftRemoteData>();
            _disposables = new CompositeDisposable();
        }

        public void SeedShaftRemoteData(ShaftRemoteData shaftRemoteData)
        {
            this.ShaftRemoteData = shaftRemoteData;

            if (shaftRemoteData.ShaftLevelData == null)
            {
                shaftRemoteData.ShaftLevelData = _staticDataModel.GetShaftLevelData(shaftRemoteData.ShaftId,
                                                                                    shaftRemoteData.ShaftLevel);
            }

            Observable.Timer(TimeSpan.FromSeconds(1)).Repeat().Subscribe((interval) => Tick()).AddTo(_disposables);
        }

        public void SetBinCash(double cash)
        {
            ShaftRemoteData.BinCash = cash;
        }

        public double RemoveBinCash(double cash)
        {
            if (cash > ShaftRemoteData.BinCash)
            {
                cash = ShaftRemoteData.BinCash;
            }

            SetBinCash(ShaftRemoteData.BinCash - cash);

            return cash;
        }

        private void Tick()
        {
            if (!(ShaftRemoteData == null || ShaftRemoteData.ShaftLevelData == null))
            {
                foreach (MinerRemoteData miner in ShaftRemoteData.Miners)
                {
                    MinerTick(miner);
                }

                ReactiveShaft.SetValueAndForceNotify(ShaftRemoteData);
            }
        }

        private void MinerTick(MinerRemoteData miner)
        {
            switch (miner.MinerState)
            {
                case EMinerState.Idle:
                    if (!string.IsNullOrEmpty(ShaftRemoteData.Manager))
                    {
                        miner.MinerState = EMinerState.WalkingToMine;
                    }
                    break;
                case EMinerState.WalkingToMine:
                    if ((miner.CurrentLocation += ShaftRemoteData.ShaftLevelData.WalkSpeed) >= _staticDataModel.MetaData.MineLength)
                    { 
                        miner.MinerState = EMinerState.Mining;
                    }
                    break;
                case EMinerState.WalkingToBin:
                    if ((miner.CurrentLocation -= ShaftRemoteData.ShaftLevelData.WalkSpeed) <= 0)
                    {
                        SetBinCash(ShaftRemoteData.BinCash + miner.MinedCash);
                        miner.MinedCash = 0;
                        miner.MinerState = ShaftRemoteData.Manager == null ? EMinerState.Idle : EMinerState.WalkingToMine;
                    }
                    break;
                case EMinerState.Mining:
                    miner.MinedCash += ShaftRemoteData.ShaftLevelData.MinningSpeed;
                    if(miner.MinedCash >= ShaftRemoteData.ShaftLevelData.WorkerCapacity)
                    {
                        miner.MinedCash = ShaftRemoteData.ShaftLevelData.WorkerCapacity;
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