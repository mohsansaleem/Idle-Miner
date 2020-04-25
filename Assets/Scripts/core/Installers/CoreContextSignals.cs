using PG.Core.Commands;
using RSG;
using Zenject;

namespace PG.Core.Installers
{
    public class LoadSceneCommandParams : BaseCommandParams
    {
        public string Scene;
        public readonly Promise OnComplete;

        public LoadSceneCommandParams()
        {
            OnComplete = new Promise();
        }
    }

    public class LoadUnloadScenesCommandParams : BaseCommandParams
    {
        public string[] LoadScenes;
        public string[] UnloadScenes;
        public readonly Promise OnComplete;

        public LoadUnloadScenesCommandParams()
        {
            OnComplete = new Promise();
        }
    }

    public class LoadSceneSignal : Signal<LoadSceneCommandParams, LoadSceneSignal>
    {
        public IPromise Load(string scene)
        {
            LoadSceneCommandParams loadParams = new LoadSceneCommandParams()
            {
                Scene = scene
            };
            
            Fire(loadParams);

            return loadParams.OnComplete;
        }
    }

    public class LoadUnloadScenesSignal : Signal<LoadUnloadScenesCommandParams, LoadUnloadScenesSignal>
    {
        public IPromise Load(string[] loadScenes)
        {
            LoadUnloadScenesCommandParams loadUnloadParams = new LoadUnloadScenesCommandParams()
            {
                LoadScenes = loadScenes,
                UnloadScenes = null
            };

            Fire(loadUnloadParams);

            return loadUnloadParams.OnComplete;
        }

        public IPromise Load(string loadScene)
        {
            LoadUnloadScenesCommandParams loadUnloadParams = new LoadUnloadScenesCommandParams()
            {
                LoadScenes = new[] { loadScene },
                UnloadScenes = null
            };

            Fire(loadUnloadParams);

            return loadUnloadParams.OnComplete;
        }

        public IPromise Unload(string[] unloadScenes)
        {
            LoadUnloadScenesCommandParams loadUnloadParams = new LoadUnloadScenesCommandParams()
            {
                LoadScenes = null,
                UnloadScenes = unloadScenes
            };

            Fire(loadUnloadParams);

            return loadUnloadParams.OnComplete;
        }

        public IPromise Unload(string unloadScene)
        {
            LoadUnloadScenesCommandParams loadUnloadParams = new LoadUnloadScenesCommandParams()
            {
                LoadScenes = null,
                UnloadScenes = new[] { unloadScene }
            };

            Fire(loadUnloadParams);

            return loadUnloadParams.OnComplete;
        }

        public IPromise LoadUnload(string[] loadScenes, string[] unloadScenes)
        {
            LoadUnloadScenesCommandParams loadUnloadParams = new LoadUnloadScenesCommandParams()
            {
                LoadScenes = loadScenes,
                UnloadScenes = unloadScenes
            };

            Fire(loadUnloadParams);

            return loadUnloadParams.OnComplete;
        }

        public IPromise LoadUnload(string loadScene, string unloadScene)
        {
            LoadUnloadScenesCommandParams loadUnloadParams = new LoadUnloadScenesCommandParams()
            {
                LoadScenes = new[] { loadScene },
                UnloadScenes = new[] { unloadScene }
            };

            Fire(loadUnloadParams);

            return loadUnloadParams.OnComplete;
        }
    }

    public class UnloadSceneSignal : Signal<LoadSceneCommandParams, UnloadSceneSignal>
    {
        public IPromise Unload(string scene)
        {
            LoadSceneCommandParams unloadParams = new LoadSceneCommandParams()
            {
                Scene = scene
            };

            Fire(unloadParams);

            return unloadParams.OnComplete;
        }
    }

    public class UnloadAllScenesExceptSignal : Signal<LoadSceneCommandParams, UnloadSceneSignal>
    {
        public IPromise UnloadAllExcept(string scene)
        {
            LoadSceneCommandParams unloadParams = new LoadSceneCommandParams()
            {
                Scene = scene
            };

            Fire(unloadParams);

            return unloadParams.OnComplete;
        }
    }
}