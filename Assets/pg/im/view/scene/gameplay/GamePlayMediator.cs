using pg.core;
using pg.im.model.remote;
using pg.im.model.scene;
using System;
using UniRx;
using Zenject;

namespace pg.im.view
{
    public partial class GamePlayMediator : StateMachineMediator
    {
        [Inject] private readonly GamePlayView _view;

        [Inject] private readonly GamePlayModel _gamePlayModel;
        [Inject] private readonly RemoteDataModel _remoteDataModel;

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
                shaft.BinCash.Subscribe((cash) => { UnityEngine.Debug.LogError(shaft._shaftRemoteData.ShaftId + " has cash: " + cash); }).AddTo(_disposables);
            }

            _gamePlayModel.GamePlayState.Subscribe(OnLoadingProgressChanged).AddTo(_disposables);
        }

        private void OnLoadingProgressChanged(GamePlayModel.EGamePlayState gamePlayState)
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

