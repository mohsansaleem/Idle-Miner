using pg.core;
using pg.core.installer;
using pg.im.installer;
using pg.im.model;
using pg.im.view;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using RSG;
using Zenject;
using System.IO;
using pg.im.view.scene;
using pg.im.view.popup.popupconfig;
using pg.im.view.popup.popupresult;

namespace pg.im.view
{
    public partial class GamePlayMediator : StateMachineMediator
    {
        [Inject] private readonly GamePlayView _view;

        [Inject] private readonly GamePlayModel _gamePlayModel;

        public GamePlayMediator()
        {
            _disposables = new CompositeDisposable();
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _stateBehaviours.Add(typeof(GamePlayStateDefault), new GamePlayStateDefault(this));

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

