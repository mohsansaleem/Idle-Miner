using pg.core;
using pg.core.installer;
using pg.im.installer;
using System;
using UniRx;
using UnityEngine;
using Zenject;
using pg.im.view.scene;
using pg.im.model.remote;
using pg.im.model.scene;

namespace pg.im.view
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

        private void SeedData()
        {
            //LoginData loginData = _remoteDataModel.LoginData;
            //_workshopRemoteDataModel.WorkshopData = loginData.WorkshopData;
            //_workshopRemoteDataModel.SeedModules(loginData.PlayerOwnedModules);
            //_workshopRemoteDataModel.SeedWorkers(loginData.PlayerOwnedWorkers);
            //_workshopRemoteDataModel.SeedHeroes(loginData.PlayerOwnerHeroes);
            //_workshopRemoteDataModel.SeedPlacementGrids(loginData.PlacementGrids);
            
            //_playerDataModel.SeedPlayerData(loginData.Player);
            //_playerDataModel.SeedResources(loginData.Resources);
            //_itemsRemoteDataModel.SeedBlueprints(loginData.PlayerOwnedBlueprints);
            //_itemsRemoteDataModel.SeedInventoryItems(loginData.ItemInventory);
            //_jobsRemoteDataModel.SeedCraftingJobs(loginData.CraftingJobs);
            //_jobsRemoteDataModel.SeedJobSlots(loginData.CraftingSlots);

            //// TODO: MS: Discuss with Chase where to do this. In a Loading Scene, I think.
            //_getMyTeamsSignal.Fire(new GetMyTeamsSignalParams() { TeamType = TeamType.City });

            //_startupDataModel.LoadingProgress.Value = StartupDataModel.ELoadingProgress.MoveToGamePlay;

            //// now that we have both remote & static data loaded & ready,
            //// we need to link up all the player owned modules with their module type datas
            //for (int i = 0; i < loginData.PlayerOwnedModules.Count; i++)
            //{
            //    ModuleData moduleData = loginData.PlayerOwnedModules [i];

            //    // @todo - chase - make sure this is set before you commit asset bundles.
            //    moduleData.ModuleTypeData = _staticDataModel.GetModuleByName (moduleData.Name);
            //}

            //// also link up all the workers with their worker types
            //for (int i = 0; i < loginData.PlayerOwnedWorkers.Count; i++) {
            //    WorkerData workerData = loginData.PlayerOwnedWorkers [i];
            //    workerData.WorkerTypeData = _staticDataModel.GetWorkerTypeByName (workerData.Name);
            //}

            //// Linking up all the Heros with their HeroTypes.
            //for (int i = 0; i < loginData.PlayerOwnerHeroes.Count; i++)
            //{
            //    HeroData heroData = loginData.PlayerOwnerHeroes[i];
            //    heroData.HeroTypeData = _staticDataModel.GetHeroTypeByName(heroData.Name);
            //}
            
            //_staticDataModel.BackgroundLayout.Value = loginData.Background;
        }

        private void OnReload()
        {
            _unloadAllScenesExceptSignal.UnloadAllExcept(Scenes.Startup).Done
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

