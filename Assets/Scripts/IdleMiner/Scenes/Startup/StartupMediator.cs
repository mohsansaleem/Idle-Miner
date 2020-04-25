using System;
using PG.Core.FSM;
using PG.Core.Installers;
using PG.IdleMiner.Models.MediatorModels;
using PG.IdleMiner.Models.RemoteDataModels;
using UniRx;
using UnityEngine;
using Zenject;

namespace PG.IdleMiner.Scenes.Startup
{
    public partial class StartupMediator : StateMachineMediator
    {
        [Inject] private readonly StartupView _view;

        [Inject] private readonly StartupModel _startupModel;
        [Inject] private readonly RemoteDataModel _remoteDataModel;
        
        [Inject] private readonly LoadUnloadScenesSignal _loadUnloadScenesSignal;
        [Inject] private readonly UnloadSceneSignal _unloadSceneSignal;
        [Inject] private readonly UnloadAllScenesExceptSignal _unloadAllScenesExceptSignal;
        [Inject] private readonly LoadStaticDataSignal _loadStaticDataSignal;
        [Inject] private readonly LoadUserDataSignal _loadUserDataSignal;
        [Inject] private readonly SaveUserDataSignal _saveUserDataSignal;
        [Inject] private readonly CreateUserDataSignal _createUserDataSignal;

        public StartupMediator()
        {
            _disposables = new CompositeDisposable();
        }

        public override void Initialize()
        {
            base.Initialize();

            // TODO: Odd collection. Figure something out for states tracking.
            _stateBehaviours.Add(typeof(StartupStateLoadPopup), new StartupStateLoadPopup(this));
            _stateBehaviours.Add(typeof(StartupStateLoadStaticData), new StartupStateLoadStaticData(this));
            _stateBehaviours.Add(typeof(StartupStateLoadUserData), new StartupStateLoadUserData(this));
            _stateBehaviours.Add(typeof(StartupStateCreateUserData), new StartupStateCreateUserData(this));
            _stateBehaviours.Add(typeof(StartupStateLoadHud), new StartupStateLoadHud(this));
            _stateBehaviours.Add(typeof(StartupStateLoadGamePlay), new StartupStateLoadGamePlay(this));
            _stateBehaviours.Add(typeof(StartupStateGamePlay), new StartupStateGamePlay(this));

            _startupModel.LoadingProgress.Subscribe(OnLoadingProgressChanged).AddTo(_disposables);
        }

        private void OnLoadingProgressChanged(StartupModel.ELoadingProgress loadingProgress)
        {
            _view.ProgressBar.value = (float)loadingProgress / 100;


            Type targetType = null;
            switch (loadingProgress)
            {
                case StartupModel.ELoadingProgress.Zero:
                    targetType = typeof(StartupStateLoadPopup);
                    break;
                case StartupModel.ELoadingProgress.PopupLoaded:
                    targetType = typeof(StartupStateLoadStaticData);
                    break;
                case StartupModel.ELoadingProgress.StaticDataLoaded:
                    targetType = typeof(StartupStateLoadUserData);
                    break;
                case StartupModel.ELoadingProgress.UserNotFound:
                    targetType = typeof(StartupStateCreateUserData);
                    break;
                case StartupModel.ELoadingProgress.DataSeeded:
                    targetType = typeof(StartupStateLoadHud);
                    break;
                case StartupModel.ELoadingProgress.HudLoaded:
                    targetType = typeof(StartupStateLoadGamePlay);
                    break;
                case StartupModel.ELoadingProgress.GamePlayLoaded:
                    targetType = typeof(StartupStateGamePlay);
                    break;
            }

            if (targetType != null &&
                (_currentStateBehaviour == null ||
                 targetType != _currentStateBehaviour.GetType()))
            {
                GoToState(targetType);
            }
        }

        private void OnReload()
        {
            _unloadAllScenesExceptSignal.UnloadAllExcept(ProjectScenes.Startup).Done
            (
                () =>
                {
                    _startupModel.LoadingProgress.Value = StartupModel.ELoadingProgress.Zero;
                },
                exception =>
                {
                    Debug.LogError("Error While Reloading: " + exception.ToString());
                }
            );
        }

        private void OnLoadingStart()
        {
            _view.Show();
        }
    }
}

