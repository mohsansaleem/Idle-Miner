using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using pg.core.installer;
using System.Collections.Generic;
using RSG;
using System;

namespace pg.core.view
{
    public enum ChangeSceneScenario
    {
        AllButLoaderChange = 0,
        OnScreenChange = 1,
        OnHudChange = 2,
        OnPopupChange = 3,
        Always = 4
    }

    public enum ChangeSceneSaveBehaviour
    {
        None = 0,
        SaveScreen = 1,
        SaveScreenAndPopups = 2,
        SaveScreenAndHud = 3,
        SaveAll = 4,
        NukeHistoryStack = 5
    }

    public enum ChangeSceneCloseBehaviour
    {
        None = 0,
        CloseScreen = 1,
        CloseHud = 2,
        CloseScreenAndHud = 3,
        ClosePopups = 4,
        CloseScreenAndPopups = 5,
        ClosePopupsAndHud = 6,
        CloseAll = 7
    }
    
    public class ChangeSceneStateBehaviour
    {
        public ChangeSceneStateBehaviour(
            ChangeSceneScenario scenario,
            ChangeSceneSaveBehaviour saveBehaviour,
            ChangeSceneCloseBehaviour closeBehaviour)
        {
            Scenario = scenario;
            SaveBehaviour = saveBehaviour;
            CloseBehaviour = closeBehaviour;
        }

        public readonly ChangeSceneScenario Scenario;
        public readonly ChangeSceneSaveBehaviour SaveBehaviour;
        public readonly ChangeSceneCloseBehaviour CloseBehaviour;
    }
    
    public class BaseSceneParams
    {

    }

    public class CoreSceneManager : MonoBehaviour
    {
        private class SceneCacheData
        {
            public CoreSceneInstaller SceneInstaller;
            public float CacheSeconds;

            public SceneCacheData(CoreSceneInstaller sceneInstaller)
            {
                SceneInstaller = sceneInstaller;
                CacheSeconds = 0f;
            }
        }

        private class SceneOpeningData
        {
            public readonly string Path;
            public readonly Type OpenState;
            public readonly BaseSceneParams OpenParams;
            public readonly ChangeSceneStateBehaviour[] ChangeSceneStateBehaviours;
            private Promise _loadPromise;
            public Promise LoadPromise
            {
                get { return _loadPromise; }
            }

            public SceneOpeningData(
                string path,
                Type openState,
                BaseSceneParams openParams,
                ChangeSceneStateBehaviour[] changeSceneStateBehaviours)
            {
                Path = path;
                OpenState = openState;
                OpenParams = openParams;
                ChangeSceneStateBehaviours = changeSceneStateBehaviours;
                _loadPromise = new Promise();
            }

            public void ClearPromise()
            {
                _loadPromise = null;
            }

            public void GetBehavioursForSceneType(
                SceneType sceneType, 
                out ChangeSceneSaveBehaviour saveBehaviour, 
                out ChangeSceneCloseBehaviour closeBehaviour)
            {
                saveBehaviour = ChangeSceneSaveBehaviour.None;
                closeBehaviour = ChangeSceneCloseBehaviour.None;

                if (ChangeSceneStateBehaviours == null)
                {
                    return;
                }

                ChangeSceneStateBehaviour stateBehaviour;
                switch (sceneType)
                {
                    case SceneType.Screen:
                        for (int i = 0; i < ChangeSceneStateBehaviours.Length; i++)
                        {
                            stateBehaviour = ChangeSceneStateBehaviours[i];
                            switch (stateBehaviour.Scenario)
                            {
                                case ChangeSceneScenario.AllButLoaderChange:
                                case ChangeSceneScenario.Always:
                                case ChangeSceneScenario.OnScreenChange:
                                    saveBehaviour = CombineSaveBehaviours(stateBehaviour.SaveBehaviour, saveBehaviour);
                                    closeBehaviour = CombineCloseBehaviours(stateBehaviour.CloseBehaviour, closeBehaviour);
                                    break;
                            }
                        }
                        break;
                    case SceneType.HUD:
                        for (int i = 0; i < ChangeSceneStateBehaviours.Length; i++)
                        {
                            stateBehaviour = ChangeSceneStateBehaviours[i];
                            switch (stateBehaviour.Scenario)
                            {
                                case ChangeSceneScenario.AllButLoaderChange:
                                case ChangeSceneScenario.Always:
                                case ChangeSceneScenario.OnHudChange:
                                    saveBehaviour = CombineSaveBehaviours(stateBehaviour.SaveBehaviour, saveBehaviour);
                                    closeBehaviour = CombineCloseBehaviours(stateBehaviour.CloseBehaviour, closeBehaviour);
                                    break;
                            }
                        }
                        break;
                    case SceneType.Popup:
                        for (int i = 0; i < ChangeSceneStateBehaviours.Length; i++)
                        {
                            stateBehaviour = ChangeSceneStateBehaviours[i];
                            switch (stateBehaviour.Scenario)
                            {
                                case ChangeSceneScenario.AllButLoaderChange:
                                case ChangeSceneScenario.Always:
                                case ChangeSceneScenario.OnPopupChange:
                                    saveBehaviour = CombineSaveBehaviours(stateBehaviour.SaveBehaviour, saveBehaviour);
                                    closeBehaviour = CombineCloseBehaviours(stateBehaviour.CloseBehaviour, closeBehaviour);
                                    break;
                            }
                        }
                        break;
                    case SceneType.Loader:
                    default:
                        for (int i = 0; i < ChangeSceneStateBehaviours.Length; i++)
                        {
                            stateBehaviour = ChangeSceneStateBehaviours[i];
                            switch (stateBehaviour.Scenario)
                            {
                                case ChangeSceneScenario.Always:
                                    saveBehaviour = CombineSaveBehaviours(stateBehaviour.SaveBehaviour, saveBehaviour);
                                    closeBehaviour = stateBehaviour.CloseBehaviour;
                                    break;
                            }
                        }
                        break;
                }
            }

            private ChangeSceneSaveBehaviour CombineSaveBehaviours(
                ChangeSceneSaveBehaviour saveBehaviour1, 
                ChangeSceneSaveBehaviour saveBehaviour2)
            {
                switch (saveBehaviour1)
                {
                    case ChangeSceneSaveBehaviour.NukeHistoryStack:
                        return saveBehaviour1;
                    case ChangeSceneSaveBehaviour.SaveAll:
                        if (saveBehaviour2 == ChangeSceneSaveBehaviour.NukeHistoryStack)
                        {
                            return saveBehaviour2;
                        }
                        return saveBehaviour1;
                    case ChangeSceneSaveBehaviour.SaveScreenAndPopups:
                        switch (saveBehaviour2)
                        {
                            case ChangeSceneSaveBehaviour.NukeHistoryStack:
                            case ChangeSceneSaveBehaviour.SaveAll:
                                return saveBehaviour2;
                            case ChangeSceneSaveBehaviour.SaveScreenAndHud:
                                return ChangeSceneSaveBehaviour.SaveAll;
                            default:
                                return saveBehaviour1;
                        }
                    case ChangeSceneSaveBehaviour.SaveScreenAndHud:
                        switch (saveBehaviour2)
                        {
                            case ChangeSceneSaveBehaviour.NukeHistoryStack:
                            case ChangeSceneSaveBehaviour.SaveAll:
                                return saveBehaviour2;
                            case ChangeSceneSaveBehaviour.SaveScreenAndPopups:
                                return ChangeSceneSaveBehaviour.SaveAll;
                            default:
                                return saveBehaviour1;
                        }
                    case ChangeSceneSaveBehaviour.SaveScreen:
                        switch (saveBehaviour2)
                        {
                            case ChangeSceneSaveBehaviour.NukeHistoryStack:
                            case ChangeSceneSaveBehaviour.SaveAll:
                            case ChangeSceneSaveBehaviour.SaveScreenAndPopups:
                            case ChangeSceneSaveBehaviour.SaveScreenAndHud:
                                return saveBehaviour2;
                            default:
                                return saveBehaviour1;
                        }
                    case ChangeSceneSaveBehaviour.None:
                    default:
                        return saveBehaviour2;
                }
            }

            private ChangeSceneCloseBehaviour CombineCloseBehaviours(
                ChangeSceneCloseBehaviour closeBehaviour1,
                ChangeSceneCloseBehaviour closeBehaviour2)
            {
                switch (closeBehaviour1)
                {
                    case ChangeSceneCloseBehaviour.CloseAll:
                        return closeBehaviour1;
                    case ChangeSceneCloseBehaviour.ClosePopupsAndHud:
                        switch (closeBehaviour2)
                        {
                            case ChangeSceneCloseBehaviour.CloseAll:
                                return closeBehaviour2;
                            case ChangeSceneCloseBehaviour.CloseScreenAndPopups:
                            case ChangeSceneCloseBehaviour.CloseScreenAndHud:
                            case ChangeSceneCloseBehaviour.CloseScreen:
                                return ChangeSceneCloseBehaviour.CloseAll;
                            default:
                                return closeBehaviour1;
                        }
                    case ChangeSceneCloseBehaviour.CloseScreenAndPopups:
                        switch (closeBehaviour2)
                        {
                            case ChangeSceneCloseBehaviour.CloseAll:
                                return closeBehaviour2;
                            case ChangeSceneCloseBehaviour.ClosePopupsAndHud:
                            case ChangeSceneCloseBehaviour.CloseScreenAndHud:
                            case ChangeSceneCloseBehaviour.CloseHud:
                                return ChangeSceneCloseBehaviour.CloseAll;
                            default:
                                return closeBehaviour1;
                        }
                    case ChangeSceneCloseBehaviour.CloseScreenAndHud:
                        switch (closeBehaviour2)
                        {
                            case ChangeSceneCloseBehaviour.CloseAll:
                                return closeBehaviour2;
                            case ChangeSceneCloseBehaviour.ClosePopupsAndHud:
                            case ChangeSceneCloseBehaviour.CloseScreenAndPopups:
                            case ChangeSceneCloseBehaviour.ClosePopups:
                                return ChangeSceneCloseBehaviour.CloseAll;
                            default:
                                return closeBehaviour1;
                        }
                    case ChangeSceneCloseBehaviour.ClosePopups:
                        switch (closeBehaviour2)
                        {
                            case ChangeSceneCloseBehaviour.CloseAll:
                            case ChangeSceneCloseBehaviour.ClosePopupsAndHud:
                            case ChangeSceneCloseBehaviour.CloseScreenAndPopups:
                                return closeBehaviour2;
                            case ChangeSceneCloseBehaviour.CloseScreenAndHud:
                                return ChangeSceneCloseBehaviour.CloseAll;
                            case ChangeSceneCloseBehaviour.CloseHud:
                                return ChangeSceneCloseBehaviour.ClosePopupsAndHud;
                            case ChangeSceneCloseBehaviour.CloseScreen:
                                return ChangeSceneCloseBehaviour.CloseScreenAndPopups;
                            default:
                                return closeBehaviour1;
                        }
                    case ChangeSceneCloseBehaviour.CloseHud:
                        switch (closeBehaviour2)
                        {
                            case ChangeSceneCloseBehaviour.CloseAll:
                            case ChangeSceneCloseBehaviour.ClosePopupsAndHud:
                            case ChangeSceneCloseBehaviour.CloseScreenAndHud:
                                return closeBehaviour2;
                            case ChangeSceneCloseBehaviour.CloseScreenAndPopups:
                                return ChangeSceneCloseBehaviour.CloseAll;
                            case ChangeSceneCloseBehaviour.ClosePopups:
                                return ChangeSceneCloseBehaviour.ClosePopupsAndHud;
                            case ChangeSceneCloseBehaviour.CloseScreen:
                                return ChangeSceneCloseBehaviour.CloseScreenAndHud;
                            default:
                                return closeBehaviour1;
                        }
                    case ChangeSceneCloseBehaviour.CloseScreen:
                        switch (closeBehaviour2)
                        {
                            case ChangeSceneCloseBehaviour.CloseAll:
                            case ChangeSceneCloseBehaviour.CloseScreenAndPopups:
                            case ChangeSceneCloseBehaviour.CloseScreenAndHud:
                                return closeBehaviour2;
                            case ChangeSceneCloseBehaviour.ClosePopupsAndHud:
                                return ChangeSceneCloseBehaviour.CloseAll;
                            case ChangeSceneCloseBehaviour.ClosePopups:
                                return ChangeSceneCloseBehaviour.CloseScreenAndPopups;
                            case ChangeSceneCloseBehaviour.CloseHud:
                                return ChangeSceneCloseBehaviour.CloseScreenAndHud;
                            default:
                                return closeBehaviour1;
                        }
                    case ChangeSceneCloseBehaviour.None:
                    default:
                        return closeBehaviour2;
                }
            }
        }

        private class SceneStateData
        {
            public string ScreenPath;
            public Type ScreenState;
            public BaseSceneParams ScreenParams;

            public string HudPath;
            public Type HudState;
            public BaseSceneParams HudParams;

            public string[] PopupPaths;
            public Type[] PopupStates;
            public BaseSceneParams[] PopupParams;

            public SceneStateData(
                string screenPath,
                Type screenState,
                BaseSceneParams screenParams)
            {
                ScreenPath = screenPath;
                ScreenState = screenState;
                ScreenParams = screenParams;
            }
        }


        public float MaxCacheWeight = 100f;
        public bool InfiniteCacheUnderMaxWeight = true;



        [Inject] private readonly ISceneLoader _sceneLoader;

        private List<SceneOpeningData> _openingScenes;

        private Dictionary<string, SceneCacheData> _cachedScenes;

        private CoreSceneInstaller _activeScreen = null;
        private List<SceneStateData> _sceneStateHistoryStack;

        private CoreSceneInstaller _activeHUD = null;
        private List<CoreSceneInstaller> _activePopups = null;
        private CoreSceneInstaller _activeLoader = null;
        private Promise _activeLoaderClosePromise = null;

        private int _currentPopupCameraDepth = _basePopupCameraDepth;

        //Max cameras must be < 1/2 depth seperation
        private const int _maxCamerasPerScene = 5;
        private const int _screenCameraDepth = -200;
        private const int _hudCameraDepth = -100;
        private const int _basePopupCameraDepth = 100;
        private const int _loaderCameraDepth = 1000;


        private ChangeSceneStateBehaviour[] _defaultChangeSceneStateBehaviours;

        protected virtual string GenerateScenePath(string sceneName)
        {
            throw new NotImplementedException();
        }

        public IPromise OpenScene(
            string sceneName,
            ChangeSceneStateBehaviour[] changeSceneStateBehaviours = null)
        {
            if (changeSceneStateBehaviours == null)
            {
                changeSceneStateBehaviours = GetDefaultStateBehaviours();
            }
            return InternalOpenScene(
                GenerateScenePath(sceneName), 
                null, 
                null,
                changeSceneStateBehaviours);
        }
        
        public IPromise OpenScene(
            string sceneName,
            Type openState,
            ChangeSceneStateBehaviour[] changeSceneStateBehaviours = null)
        {
            if (changeSceneStateBehaviours == null)
            {
                changeSceneStateBehaviours = GetDefaultStateBehaviours();
            }
            return InternalOpenScene(
                GenerateScenePath(sceneName), 
                openState, 
                null,
                changeSceneStateBehaviours);
        }

        public IPromise OpenScene(
            string sceneName,
            BaseSceneParams openParams,
            ChangeSceneStateBehaviour[] changeSceneStateBehaviours = null)
        {
            if (changeSceneStateBehaviours == null)
            {
                changeSceneStateBehaviours = GetDefaultStateBehaviours();
            }
            return InternalOpenScene(
                GenerateScenePath(sceneName), 
                null, 
                openParams,
                changeSceneStateBehaviours);
        }
        
        public IPromise OpenScene(
            string sceneName,
            Type openState,
            BaseSceneParams openParams,
            ChangeSceneStateBehaviour[] changeSceneStateBehaviours = null)
        {
            if (changeSceneStateBehaviours == null)
            {
                changeSceneStateBehaviours = GetDefaultStateBehaviours();
            }
            return InternalOpenScene(
                GenerateScenePath(sceneName),
                openState,
                openParams,
                changeSceneStateBehaviours);
        }

        public void OpenPreviousSceneState()
        {
            if (_sceneStateHistoryStack != null && _sceneStateHistoryStack.Count > 1)
            {
                int topSceneIndex = _sceneStateHistoryStack.Count - 1;
                _sceneStateHistoryStack.RemoveAt(topSceneIndex);
                topSceneIndex--;

                //Close down current scene state
                //(leave current screen for possible reuse in previous state)
                InternalCloseHud();
                InternalClosePopups();

                //Show all scenes in previous scene state

                SceneStateData sceneState = _sceneStateHistoryStack[topSceneIndex];

                InternalOpenScene(
                    sceneState.ScreenPath, 
                    sceneState.ScreenState,
                    sceneState.ScreenParams,
                    null);

                if (!string.IsNullOrEmpty(sceneState.HudPath))
                {
                    InternalOpenScene(
                        sceneState.HudPath, 
                        sceneState.HudState,
                        sceneState.HudParams,
                        null);
                }

                if (sceneState.PopupPaths != null)
                {
                    //Show popups in ascending order for proper layering
                    for (int i = 0; i < sceneState.PopupPaths.Length; i++)
                    {
                        InternalOpenScene(
                            sceneState.PopupPaths[i], 
                            sceneState.PopupStates[i],
                            sceneState.PopupParams[i],
                            null);
                    }
                }
            }
            else
            {
                //TODO: Implement behaviour for going back when on starting screen.
                //Process.GetCurrentProcess().Kill();
            }
        }

        private ChangeSceneStateBehaviour[] GetDefaultStateBehaviours()
        {
            if (_defaultChangeSceneStateBehaviours == null)
            {
                ChangeSceneStateBehaviour screenChangeBehaviour = new ChangeSceneStateBehaviour(
                    ChangeSceneScenario.OnScreenChange,
                    ChangeSceneSaveBehaviour.SaveAll,
                    ChangeSceneCloseBehaviour.ClosePopupsAndHud);

                ChangeSceneStateBehaviour popupChangeBehaviour = new ChangeSceneStateBehaviour(
                    ChangeSceneScenario.OnPopupChange,
                    ChangeSceneSaveBehaviour.SaveAll,
                    ChangeSceneCloseBehaviour.None);

                ChangeSceneStateBehaviour hudChangeBehaviour = new ChangeSceneStateBehaviour(
                    ChangeSceneScenario.OnHudChange,
                    ChangeSceneSaveBehaviour.SaveAll,
                    ChangeSceneCloseBehaviour.None);
                
                _defaultChangeSceneStateBehaviours = new [] { screenChangeBehaviour, popupChangeBehaviour, hudChangeBehaviour };
            }
            return _defaultChangeSceneStateBehaviours;
        }

        private IPromise InternalOpenScene(
            string scenePath,
            Type openState,
            BaseSceneParams openParams,
            ChangeSceneStateBehaviour[] changeSceneStateBehaviours)
        {
            if(_openingScenes == null)
            {
                _openingScenes = new List<SceneOpeningData>();
            }

            SceneOpeningData openData = new SceneOpeningData(
                scenePath,
                openState,
                openParams,
                changeSceneStateBehaviours
            );
            _openingScenes.Add(openData);

            //If new scene is not waiting in an open queue, load now
            if (_openingScenes.Count == 1)
            {
                OpenNextSceneInQueue();
            }
            return openData.LoadPromise;
        }

        private void OpenNextSceneInQueue()
        {
            if(_openingScenes == null || _openingScenes.Count == 0)
            {
                return;
            }

            SceneOpeningData openData = _openingScenes[0];

            //Only popups are allowed to load multiple instances of the same scene
            if (_activeScreen != null && _activeScreen.gameObject.scene.path == openData.Path)
            {
                //This Screen is already an active loaded scene
                //Still check for closing all popups and changing scene state
                InternalSceneOpeningChecks(_activeScreen);
                InternalUpdateSceneStateHistory(_activeScreen, openData);
                OpenNextSceneInQueue();
                return;
            }
            else if (_activeHUD != null && _activeHUD.gameObject.scene.path == openData.Path)
            {
                //This HUD is already an active loaded scene
                //Still check for closing all popups and changing scene state
                InternalSceneOpeningChecks(_activeHUD);
                InternalUpdateSceneStateHistory(_activeHUD, openData);
                OpenNextSceneInQueue();
                return;
            }
            else if (_activeLoader != null && _activeLoader.gameObject.scene.path == openData.Path)
            {
                //This Loader is already an active loaded scene
                //Still check for closing all popups and changing scene state
                InternalSceneOpeningChecks(_activeLoader);
                InternalUpdateSceneStateHistory(_activeLoader, openData);
                OpenNextSceneInQueue();
                return;
            }

            CoreSceneInstaller sceneInstaller = TryActivateSceneFromCache(openData.Path);
            if (sceneInstaller != null)
            {
                //Loaded scene from cache
                RegisterSceneInstaller(sceneInstaller);
                return;
            }

            _sceneLoader.LoadScene(openData.Path).Then(
                () =>
                {
                    Debug.Log(string.Format("{0} , '{1}' scene loading completed!", this, openData.Path));

                    //Do not resolve load promises here, allow InternalSceneOpeningChecks to do it.
                },
                exception =>
                {
                    Debug.LogWarning(string.Format("{0} , '{1}' scene failed to load!", this, openData.Path));

                    if (openData.LoadPromise != null)
                    {
                        openData.LoadPromise.Reject(exception);
                        openData.ClearPromise();
                    }
                },
                progress =>
                {
                    if (openData.LoadPromise != null)
                    {
                        openData.LoadPromise.ReportProgress(progress);
                    }
                }
                );
        }

        public void CloseScene(Scene scene)
        {
            if (_activeScreen != null && _activeScreen.gameObject.scene.buildIndex == scene.buildIndex)
            {
                CloseSceneWithInstaller(_activeScreen);
            }
            else if (_activeHUD != null && _activeHUD.gameObject.scene.buildIndex == scene.buildIndex)
            {
                CloseSceneWithInstaller(_activeHUD);
            }
            else if (_activeLoader != null && _activeLoader.gameObject.scene.buildIndex == scene.buildIndex)
            {
                CloseSceneWithInstaller(_activeLoader);
            }
            else
            {
                if (_activePopups != null)
                {
                    //Most likely closing top popup,
                    //start at top of popup stack.
                    for (int i = _activePopups.Count - 1; i >= 0; i--)
                    {
                        CoreSceneInstaller activePopup = _activePopups[i];
                        if (activePopup.gameObject.scene.buildIndex == scene.buildIndex)
                        {
                            CloseSceneWithInstaller(activePopup);
                            break;
                        }
                    }
                }
            }
        }

        public void CloseSceneWithInstaller(CoreSceneInstaller sceneInstaller)
        {
            if (sceneInstaller == null)
            {
                return;
            }

            bool wasActive = false;

            switch (sceneInstaller.SceneType)
            {
                case SceneType.Screen:
                    sceneInstaller.SetSceneOrderDepth(_screenCameraDepth - _maxCamerasPerScene);
                    if (sceneInstaller == _activeScreen)
                    {
                        _activeScreen = null;
                        wasActive = true;
                    }
                    else
                    {
                        Debug.LogWarning(string.Format("{0} , '{1}' being closed but is not the active screen!", this, sceneInstaller.gameObject.scene.name));
                    }
                    break;
                case SceneType.HUD:
                    sceneInstaller.SetSceneOrderDepth(_hudCameraDepth - _maxCamerasPerScene);
                    if (sceneInstaller == _activeHUD)
                    {
                        _activeHUD = null;
                        wasActive = true;
                    }
                    else
                    {
                        Debug.LogWarning(string.Format("{0} , '{1}' being closed but is not the active hud!", this, sceneInstaller.gameObject.scene.name));
                    }
                    break;
                case SceneType.Popup:
                    if (_activePopups != null && _activePopups.Contains(sceneInstaller))
                    {
                        _activePopups.Remove(sceneInstaller);
                        wasActive = true;

                        if (_activePopups.Count == 0)
                        {
                            //All popups closed, reset popups depth
                            _currentPopupCameraDepth = _basePopupCameraDepth;
                        }
                    }
                    else
                    {
                        Debug.LogWarning(string.Format("{0} , '{1}' being closed but is not an active popup!", this, sceneInstaller.gameObject.scene.name));
                    }
                    break;
                case SceneType.Loader:
                    sceneInstaller.SetSceneOrderDepth(_loaderCameraDepth - _maxCamerasPerScene);
                    if (sceneInstaller == _activeLoader)
                    {
                        _activeLoader = null;
                        if (_activeLoaderClosePromise != null)
                        {
                            _activeLoaderClosePromise.Resolve();
                            _activeLoaderClosePromise = null;
                        }
                        wasActive = true;
                    }
                    else
                    {
                        Debug.LogWarning(string.Format("{0} , '{1}' being closed but is not the active loader!", this, sceneInstaller.gameObject.scene.name));
                    }
                    break;
            }

            if(wasActive)
            {
                sceneInstaller.Close().Done(
                    () =>
                    {
                        DeactivateSceneToCache(sceneInstaller);
                    },
                    exception =>
                    {
                        _sceneLoader.UnloadScene(sceneInstaller.gameObject.scene);
                    }
                );
            }
        }


        public void RegisterSceneInstaller(CoreSceneInstaller sceneInstaller)
        {
            SceneOpeningData openData = InternalSceneOpeningChecks(sceneInstaller);
            if (openData == null)
            {
                Debug.LogWarning(string.Format("{0} , '{1}' scene registered out of order!", this, sceneInstaller.gameObject.scene.name));
            }

            switch (sceneInstaller.SceneType)
            {
                case SceneType.Screen:
                    CloseSceneWithInstaller(_activeScreen);
                    _activeScreen = sceneInstaller;
                    _activeScreen.SetSceneOrderDepth(_screenCameraDepth);
                    InternalUpdateSceneStateHistory(sceneInstaller, openData);
                    break;
                case SceneType.HUD:
                    CloseSceneWithInstaller(_activeHUD);
                    _activeHUD = sceneInstaller;
                    _activeHUD.SetSceneOrderDepth(_hudCameraDepth);
                    InternalUpdateSceneStateHistory(sceneInstaller, openData);
                    break;
                case SceneType.Popup:
                    if (_activePopups == null)
                    {
                        _activePopups = new List<CoreSceneInstaller>();
                    }
                    _activePopups.Add(sceneInstaller);
                    sceneInstaller.SetSceneOrderDepth(_currentPopupCameraDepth);
                    _currentPopupCameraDepth += _maxCamerasPerScene;
                    InternalUpdateSceneStateHistory(sceneInstaller, openData);
                    break;
                case SceneType.Loader:
                    CloseSceneWithInstaller(_activeLoader);
                    _activeLoader = sceneInstaller;
                    _activeLoader.SetSceneOrderDepth(_loaderCameraDepth);
                    InternalUpdateSceneStateHistory(sceneInstaller, openData);
                    break;
            }

            if (_activeLoaderClosePromise == null || sceneInstaller.SceneType == SceneType.Loader)
            {
                OpenSceneInstaller(sceneInstaller);
            }
            else
            {
                _activeLoaderClosePromise.Then(
                    () =>
                    {
                        OpenSceneInstaller(sceneInstaller);
                    });
            }

            OpenNextSceneInQueue();
        }

        private void OpenSceneInstaller(CoreSceneInstaller sceneInstaller)
        {
            sceneInstaller.Open().Done(
                () =>
                {
                    Debug.Log(string.Format("{0} , '{1}' scene opened!", this, sceneInstaller.gameObject.scene.name));
                    OnOpenSceneInstallerFinished(sceneInstaller);
                },
                exception =>
                {
                    Debug.LogWarning(string.Format("{0} , '{1}' scene failed to open!", this, sceneInstaller.gameObject.scene.name));
                    _sceneLoader.UnloadScene(sceneInstaller.gameObject.scene);
                }
            );
        }

        private void InternalUpdateSceneStateHistory(
            CoreSceneInstaller newSceneInstaller,
            SceneOpeningData openData)
        {
            //Note: This does not fill in the full scene state for hud/popups.
            //This simply handles adding a new scene state to the history stack
            //when needed/requested and sets the screen path/state.
            //The full hud/popups data is populated when the scene state is left
            //because it depends on what ChangeSceneStateBehaviour is specified.
            //(see SceneOpeningChecks() for details)
            if (newSceneInstaller == null)
            {
                return;
            }

            if (_sceneStateHistoryStack == null)
            {
                _sceneStateHistoryStack = new List<SceneStateData>();
            }

            bool saveRequested = false;
            BaseSceneParams openParams = null;
            if (openData != null)
            {
                ChangeSceneSaveBehaviour saveBehaviour;
                ChangeSceneCloseBehaviour closeBehaviour;
                openData.GetBehavioursForSceneType(
                    newSceneInstaller.SceneType,
                    out saveBehaviour,
                    out closeBehaviour);
                
                saveRequested = ChangeSceneStateRequestsSave(saveBehaviour);
                openParams = openData.OpenParams;
            }

            if (newSceneInstaller.SceneType == SceneType.Screen)
            {
                //Always add to history stack if new screen path
                //or if open scene behaviour explicitly saves.
                int histCount = _sceneStateHistoryStack.Count;
                string newScenePath = newSceneInstaller.gameObject.scene.path;
                if (histCount == 0 ||
                    _sceneStateHistoryStack[histCount - 1].ScreenPath != newScenePath ||
                    saveRequested)
                {
                    _sceneStateHistoryStack.Add(
                        new SceneStateData(
                            newScenePath,
                            newSceneInstaller.LastValidOpenState,
                            openParams)
                            );
                }
            }
            else if (saveRequested)
            {
                //Only add to history stack for new popups/hud 
                //if open scene behaviour explicitly saves.
                if (_activeScreen != null)
                {
                    //Assign the active screen to a new
                    //scene state history entry.
                    _sceneStateHistoryStack.Add(
                        new SceneStateData(
                            _activeScreen.gameObject.scene.path,
                            _activeScreen.LastValidOpenState,
                            openParams)
                        );
                }
            }
        }

        private bool ChangeSceneStateRequestsSave(ChangeSceneSaveBehaviour saveBehaviour)
        {
            switch(saveBehaviour)
            {
                case ChangeSceneSaveBehaviour.NukeHistoryStack:
                case ChangeSceneSaveBehaviour.None:
                    return false;
            }
            return true;
        }
        
        private void OnOpenSceneInstallerFinished(CoreSceneInstaller sceneInstaller)
        {
            //Do nothing for now
        }

        private CoreSceneInstaller TryActivateSceneFromCache(string scenePathName)
        {
            if (_cachedScenes == null || !_cachedScenes.ContainsKey(scenePathName))
            {
                return null;
            }

            SceneCacheData sceneCache = _cachedScenes[scenePathName];
            _cachedScenes.Remove(scenePathName);

            Scene scene = sceneCache.SceneInstaller.gameObject.scene;

            GameObject[] rootGameObjs = scene.GetRootGameObjects();
            for (int i = 0; i < rootGameObjs.Length; i++)
            {
                rootGameObjs[i].SetActive(true);
            }

            return sceneCache.SceneInstaller;
        }

        private void DeactivateSceneToCache(CoreSceneInstaller sceneInstaller)
        {
            Scene scene = sceneInstaller.gameObject.scene;

            if(sceneInstaller.MaxCacheSeconds != 0)
            {
                //Scene has cache time (> 0) or infinite cache (< 0)

                GameObject[] rootGameObjs = scene.GetRootGameObjects();
                for (int i = 0; i < rootGameObjs.Length; i++)
                {
                    rootGameObjs[i].SetActive(false);
                }

                if (_cachedScenes == null)
                {
                    _cachedScenes = new Dictionary<string, SceneCacheData>();
                }

                string scenePath = sceneInstaller.gameObject.scene.path;
                if (!_cachedScenes.ContainsKey(scenePath))
                {
                    _cachedScenes.Add(scenePath, new SceneCacheData(sceneInstaller));
                }
                else
                {
                    //Never cache two of the same scene, but 
                    //reset the cache time for the already cached scene.
                    _cachedScenes[scenePath].CacheSeconds = 0f;
                    _sceneLoader.UnloadScene(scene);
                }
            }
            else
            {
                //Scene marked for no cache, unload
                _sceneLoader.UnloadScene(scene);
            }
        }

        private SceneOpeningData InternalSceneOpeningChecks(CoreSceneInstaller sceneInstaller)
        {
            if(sceneInstaller == null || _openingScenes == null || _openingScenes.Count == 0)
            {
                return null;
            }

            SceneOpeningData openData = _openingScenes[0];
            if (openData.Path != sceneInstaller.gameObject.scene.path)
            {
                return null;
            }

            ChangeSceneSaveBehaviour saveBehaviour;
            ChangeSceneCloseBehaviour closeBehaviour;
            openData.GetBehavioursForSceneType(
                sceneInstaller.SceneType,
                out saveBehaviour,
                out closeBehaviour);

            //Save before closing
            switch (saveBehaviour)
            {
                case ChangeSceneSaveBehaviour.NukeHistoryStack:
                    InternalNukeHistoryStack();
                    break;
                case ChangeSceneSaveBehaviour.SaveAll:
                    InternalSaveScreen();
                    InternalSaveHud();
                    InternalSavePopups();
                    break;
                case ChangeSceneSaveBehaviour.SaveScreenAndHud:
                    InternalSaveScreen();
                    InternalSaveHud();
                    break;
                case ChangeSceneSaveBehaviour.SaveScreenAndPopups:
                    InternalSaveScreen();
                    InternalSavePopups();
                    break;
                case ChangeSceneSaveBehaviour.SaveScreen:
                    InternalSaveScreen();
                    break;
            }

            //All save requests finished, close stuff
            switch (closeBehaviour)
            {
                case ChangeSceneCloseBehaviour.CloseAll:
                    InternalClosePopups();
                    InternalCloseHud();
                    InternalCloseScreen();
                    break;
                case ChangeSceneCloseBehaviour.ClosePopupsAndHud:
                    InternalClosePopups();
                    InternalCloseHud();
                    break;
                case ChangeSceneCloseBehaviour.CloseScreenAndPopups:
                    InternalClosePopups();
                    InternalCloseScreen();
                    break;
                case ChangeSceneCloseBehaviour.CloseScreenAndHud:
                    InternalCloseHud();
                    InternalCloseScreen();
                    break;
                case ChangeSceneCloseBehaviour.ClosePopups:
                    InternalClosePopups();
                    break;
                case ChangeSceneCloseBehaviour.CloseHud:
                    InternalCloseHud();
                    break;
                case ChangeSceneCloseBehaviour.CloseScreen:
                    InternalCloseScreen();
                    break;
            }
            
            sceneInstaller.GoToState(openData.OpenState, openData.OpenParams);

            if (openData.LoadPromise != null)
            {
                openData.LoadPromise.ReportProgress(1f);
                openData.LoadPromise.Resolve();
                openData.ClearPromise();
            }

            if (sceneInstaller.SceneType == SceneType.Loader && _activeLoaderClosePromise == null)
            {
                _activeLoaderClosePromise = new Promise();
            }

            _openingScenes.RemoveAt(0);

            return openData;
        }

        private void InternalSaveScreen()
        {
            if (_activeScreen != null && 
                _sceneStateHistoryStack != null && 
                _sceneStateHistoryStack.Count > 0)
            {
                SceneStateData histData = _sceneStateHistoryStack[_sceneStateHistoryStack.Count - 1];
                histData.ScreenState = _activeScreen.LastValidOpenState;
            }
        }

        private void InternalCloseScreen()
        {
            if (_activeScreen != null)
            {
                CloseSceneWithInstaller(_activeScreen);
            }
        }

        private void InternalSavePopups()
        {
            if (_sceneStateHistoryStack != null && _sceneStateHistoryStack.Count > 0)
            {
                int lastHistIndex = _sceneStateHistoryStack.Count - 1;
                SceneStateData histData = _sceneStateHistoryStack[lastHistIndex];
                
                if (_activePopups != null)
                {
                    histData.PopupPaths = new string[_activePopups.Count];
                    histData.PopupStates = new Type[_activePopups.Count];
                    histData.PopupParams = new BaseSceneParams[_activePopups.Count];
                    for (int i = 0; i < _activePopups.Count; i++)
                    {
                        histData.PopupPaths[i] = _activePopups[i].gameObject.scene.path;
                        histData.PopupStates[i] = _activePopups[i].LastValidOpenState;
                        histData.PopupParams[i] = _activePopups[i].OpenParams;
                    }
                }
                else
                {
                    histData.PopupPaths = null;
                    histData.PopupStates = null;
                    histData.PopupParams = null;
                }
            }
        }

        private void InternalClosePopups()
        {
            if (_activePopups != null)
            {
                //Start from top to avoid removal iteration errors
                for (int i = _activePopups.Count - 1; i >= 0; i--)
                {
                    CloseSceneWithInstaller(_activePopups[i]);
                }
            }
        }

        private void InternalSaveHud()
        {
            if (_sceneStateHistoryStack != null && _sceneStateHistoryStack.Count > 0)
            {
                int lastHistIndex = _sceneStateHistoryStack.Count - 1;
                SceneStateData histData = _sceneStateHistoryStack[lastHistIndex];

                if (_activeHUD != null)
                {
                    histData.HudPath = _activeHUD.gameObject.scene.path;
                    histData.HudState = _activeHUD.LastValidOpenState;
                    histData.HudParams = _activeHUD.OpenParams;
                }
                else
                {
                    histData.HudPath = null;
                    histData.HudState = null;
                    histData.HudParams = null;
                }
            }
        }

        private void InternalCloseHud()
        {
            if(_activeHUD != null)
            {
                CloseSceneWithInstaller(_activeHUD);
            }
        }

        private void InternalNukeHistoryStack()
        {
            if (_sceneStateHistoryStack != null && _sceneStateHistoryStack.Count > 0)
            {
                _sceneStateHistoryStack.Clear();
            }
        }



        void Update()
        {
            if(_cachedScenes != null && _cachedScenes.Count > 0)
            {
                //Iterate cached scenes and increment cache time.
                //Only remove one scene from cache per Tick,
                //selecting for largest cache cost (see lower comment)
                string removeKey = "";
                float removeWeight = 0f;
                float removeCacheCost = 0f;
                float totalCacheWeight = 0f;
                foreach (KeyValuePair<string, SceneCacheData> sceneCachePair in _cachedScenes)
                {
                    SceneCacheData sceneCache = sceneCachePair.Value;
                    float cacheWeight = sceneCache.SceneInstaller.CacheWeight;

                    sceneCache.CacheSeconds += Time.deltaTime;
                    totalCacheWeight += cacheWeight;

                    //Cache cost is scene weight multiplied by percent of allowed scene cache time.
                    //Values > cacheWeight represent expired cache times.
                    float cacheCost = cacheWeight * sceneCache.CacheSeconds / (float)sceneCache.SceneInstaller.MaxCacheSeconds;

                    if(removeCacheCost < cacheCost)
                    {
                        removeKey = sceneCachePair.Key;
                        removeWeight = cacheWeight;
                        removeCacheCost = cacheCost;
                    }
                }

                if(removeKey != "")
                {
                    if ((!InfiniteCacheUnderMaxWeight && removeCacheCost > removeWeight) || 
                        totalCacheWeight > MaxCacheWeight)
                    {
                        //Infinite caching off and cache time over max
                        //or, total cache weight is over max allowed
                        //Remove scene cache with highest cache cost
                        SceneCacheData sceneCache = _cachedScenes[removeKey];
                        _cachedScenes.Remove(removeKey);
                        _sceneLoader.UnloadScene(sceneCache.SceneInstaller.gameObject.scene);
                    }
                }
            }
        }
    }
}
