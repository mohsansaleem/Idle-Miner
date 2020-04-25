using System;
using PG.Core.FSM;
using PG.IdleMiner.Misc;
using PG.IdleMiner.Models.MediatorModels;
using PG.IdleMiner.Models.RemoteDataModels;
using UniRx;
using Zenject;

namespace PG.IdleMiner.Scenes.Gameplay
{
    public partial class GamePlayMediator : StateMachineMediator
    {
        [Inject] private readonly GamePlayView _view;

        [Inject] private readonly GamePlayModel _gamePlayModel;
        [Inject] private readonly RemoteDataModel _remoteDataModel;

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
                shaft.ReactiveShaft.Subscribe(_view.UpdateShaft).AddTo(_disposables);
            }

            _remoteDataModel.Shafts.ObserveAdd().Subscribe(OnShaftAdd).AddTo(_disposables);

            _remoteDataModel.Warehouse.ReactiveWarehouse.Subscribe(_view.UpdateWarehouse).AddTo(_disposables);

            _remoteDataModel.Elevator.ReactiveElevator.Subscribe((e) => { _view.UpdateElevator(e, _remoteDataModel.Shafts.Count * Constants.ShaftDistance); });

            _gamePlayModel.GamePlayState.Subscribe(OnGamePlayStateChanged).AddTo(_disposables);
        }

        public void OnShaftAdd(CollectionAddEvent<ShaftRemoteDataModel> evt)
        {
            evt.Value.ReactiveShaft.Subscribe(_view.UpdateShaft).AddTo(_disposables);
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
    }
}

