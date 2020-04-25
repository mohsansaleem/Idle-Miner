using System;
using PG.IdleMiner.Misc;
using PG.IdleMiner.Models.DataModels;
using UniRx;
using UnityEngine;
using Zenject;

namespace PG.IdleMiner.Models.RemoteDataModels
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
            ShaftRemoteData = shaftRemoteData;

            if (shaftRemoteData.ShaftLevelData == null)
            {
                shaftRemoteData.ShaftLevelData = _staticDataModel.GetShaftLevelData(shaftRemoteData.ShaftId,
                                                                                    shaftRemoteData.ShaftLevel);
            }

            // TODO: Move this to respective Facade.
            Observable.Timer(TimeSpan.FromMilliseconds(Constants.FacadesTickTime)).Repeat().Subscribe((interval) => Tick()).AddTo(_disposables);
        }

        public void Upgrade()
        {
            if (ShaftRemoteData.ShaftLevel < _staticDataModel.MetaData.Shafts[ShaftRemoteData.ShaftId].Count)
            {
                ShaftRemoteData.ShaftLevel++;
                ShaftRemoteData.ShaftLevelData = _staticDataModel.GetShaftLevelData(ShaftRemoteData.ShaftId, ShaftRemoteData.ShaftLevel);
            }
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

        // TODO: Not the best place to do this. Add respective Facades and then do this there. 
        // For now doing it here.
        float _previousStamp = float.MinValue;
        private void Tick()
        {
            if (Math.Abs(_previousStamp - float.MinValue) < 0.0001f)
            {
                _previousStamp = Time.time;
                return;
            }

            float interval = (Time.time - _previousStamp);
            _previousStamp = Time.time;
            
            if (!(ShaftRemoteData == null || ShaftRemoteData.ShaftLevelData == null))
            {
                while (ShaftRemoteData.Miners.Count < ShaftRemoteData.ShaftLevelData.Miners)
                {
                    ShaftRemoteData.Miners.Add(new MinerRemoteData());
                }
                
                foreach (MinerRemoteData miner in ShaftRemoteData.Miners)
                {
                    MinerTick(miner, interval);
                }

                ReactiveShaft.SetValueAndForceNotify(ShaftRemoteData);
            }
        }

        private void MinerTick(MinerRemoteData miner, float interval)
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
                    if ((miner.CurrentLocation += ShaftRemoteData.ShaftLevelData.WalkSpeed * interval) >= _staticDataModel.MetaData.MineLength)
                    { 
                        miner.MinerState = EMinerState.Mining;
                    }
                    break;
                case EMinerState.WalkingToBin:
                    if ((miner.CurrentLocation -= ShaftRemoteData.ShaftLevelData.WalkSpeed * interval) <= 0)
                    {
                        SetBinCash(ShaftRemoteData.BinCash + miner.MinedCash);
                        miner.MinedCash = 0;
                        miner.MinerState = ShaftRemoteData.Manager == null ? EMinerState.Idle : EMinerState.WalkingToMine;
                    }
                    break;
                case EMinerState.Mining:
                    miner.MinedCash += ShaftRemoteData.ShaftLevelData.MinningSpeed * interval;
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