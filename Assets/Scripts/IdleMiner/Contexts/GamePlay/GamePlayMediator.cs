using System;
using PG.Core.Contexts;
using PG.IdleMiner.Misc;
using PG.IdleMiner.Models;
using PG.IdleMiner.Models.MediatorModels;
using PG.IdleMiner.Models.RemoteDataModels;
using PG.IdleMiner.Views.GamePlay;
using UniRx;
using UnityEngine;
using Zenject;

namespace PG.IdleMiner.Contexts.GamePlay
{
    public partial class GamePlayMediator : StateMachineMediator
    {
        [Inject] private readonly GamePlayView _view;

        [Inject] private readonly GamePlayModel _gamePlayModel;
        [Inject] private readonly RemoteDataModel _remoteDataModel;
        [Inject] private readonly StaticDataModel _staticDataModel;

        [Inject] private readonly AddShaftSignal _addShaftSignal;

        public GamePlayMediator()
        {
            _disposables = new CompositeDisposable();
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _stateBehaviours.Add(typeof(GamePlayStateDefault), new GamePlayStateDefault(this));
            
            foreach(ShaftRemoteDataModel shaft in _remoteDataModel.Shafts)
            {
                SetupShaft(shaft);
            }

            _remoteDataModel.Shafts.ObserveAdd().Subscribe(OnShaftAdd).AddTo(_disposables);

            _remoteDataModel.Warehouse.ReactiveWarehouse.Subscribe(_view.UpdateWarehouse).AddTo(_disposables);
            _remoteDataModel.Elevator.ReactiveElevator.Subscribe((e) => { _view.UpdateElevator(e, _remoteDataModel.Shafts.Count * Constants.ShaftDistance); });
            
            _view.SubscribeUpgradeElevator(OnElevatorUpgradeRequest);
            _view.SubscribeUpgradeWareHouse(OnWareHouseUpgradeRequest);
            
            _gamePlayModel.GamePlayState.Subscribe(OnGamePlayStateChanged).AddTo(_disposables);
        }

        public void OnShaftAdd(CollectionAddEvent<ShaftRemoteDataModel> evt)
        {
            SetupShaft(evt.Value);
        }

        private void SetupShaft(ShaftRemoteDataModel shaftRemoteDataModel)
        {
            ShaftView shaftView = _view.AddShaft(shaftRemoteDataModel.ReactiveShaft.Value);
            
            shaftView.OnShaftUpgradeClick = data =>
            {
                shaftRemoteDataModel.Upgrade();
            };
            
            shaftRemoteDataModel.ReactiveShaft.Subscribe(_view.UpdateShaft).AddTo(_disposables);
        }
        
        public void OnElevatorUpgradeRequest()
        {
            Debug.Log("OnElevatorUpgradeRequest");
            _remoteDataModel.Elevator.Upgrade();
        }

        public void OnWareHouseUpgradeRequest()
        {
            _remoteDataModel.Warehouse.Upgrade();
        }

        private void OnGamePlayStateChanged(GamePlayModel.EGamePlayState gamePlayState)
        {
            Type targetType = null;
            switch (gamePlayState)
            {
                case GamePlayModel.EGamePlayState.Game:
                    targetType = typeof(GamePlayStateDefault);
                    break;
            }

            if (targetType != null &&
                (_currentStateBehaviour == null ||
                 targetType != _currentStateBehaviour.GetType()))
            {
                GoToState(targetType);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _view.UnSubscribeElevatorHouse(OnElevatorUpgradeRequest);
            _view.UnSubscribeUpgradeWareHouse(OnWareHouseUpgradeRequest);
        }
    }
}

