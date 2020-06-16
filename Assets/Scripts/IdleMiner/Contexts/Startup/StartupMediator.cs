using System;
using PG.Core.Contexts;
using PG.Core.Installers;
using PG.IdleMiner.Models.MediatorModels;
using PG.IdleMiner.Models.RemoteDataModels;
using UniRx;
using UnityEngine;
using Zenject;

namespace PG.IdleMiner.Contexts.Startup
{
    public partial class StartupMediator : StateMachineMediator
    {
        [Inject] private readonly StartupView _view;

        [Inject] private readonly StartupModel _startupModel;
        [Inject] private readonly RemoteDataModel _remoteDataModel;

        public StartupMediator()
        {
            Disposables = new CompositeDisposable();
        }

        public override void Initialize()
        {
            base.Initialize();

            // TODO: Odd collection. Figure something out for states tracking.
            StateBehaviours.Add((int)StartupModel.ELoadingProgress.LoadPopup, new StartupStateLoadPopup(this));
            StateBehaviours.Add((int)StartupModel.ELoadingProgress.LoadStaticData, new StartupStateLoadStaticData(this));
            StateBehaviours.Add((int)StartupModel.ELoadingProgress.LoadUserData, new StartupStateLoadUserData(this));
            StateBehaviours.Add((int)StartupModel.ELoadingProgress.CreateUserData, new StartupStateCreateUserData(this));
            StateBehaviours.Add((int)StartupModel.ELoadingProgress.LoadHud, new StartupStateLoadHud(this));
            StateBehaviours.Add((int)StartupModel.ELoadingProgress.LoadGamePlay, new StartupStateLoadGamePlay(this));
            StateBehaviours.Add((int)StartupModel.ELoadingProgress.GamePlay, new StartupStateGamePlay(this));

            _startupModel.LoadingProgress.Subscribe(OnLoadingProgressChanged).AddTo(Disposables);
        }

        private void OnLoadingProgressChanged(StartupModel.ELoadingProgress loadingProgress)
        {
            _view.ProgressBar.value = (float)loadingProgress / 100;

            GoToState((int)loadingProgress);
        }

        private void OnReload()
        {
            UnloadAllScenesExceptSignal.UnloadAllExcept(ProjectScenes.Startup, SignalBus).Done
            (
                () =>
                {
                    _startupModel.LoadingProgress.Value = StartupModel.ELoadingProgress.LoadPopup;
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

