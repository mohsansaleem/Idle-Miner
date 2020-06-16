using PG.Core.Commands;
using RSG;
using Zenject;

namespace PG.Core.Installers
{
    public abstract class ASignal
    {
        public readonly Promise OnComplete;

        protected ASignal()
        {
            OnComplete = new Promise();
        }

        protected IPromise Fire()
        {
            return OnComplete;
        }
    }

    public class LoadSceneSignal : ASignal
    {
        public string Scene;
        
        public static IPromise Load(string scene, SignalBus signalBus)
        {
            LoadSceneSignal loadParams = new LoadSceneSignal()
            {
                Scene = scene
            };
            
            signalBus.Fire(loadParams);

            return loadParams.Fire();
        }
    }

    public class LoadUnloadScenesSignal : ASignal
    {
        public string[] LoadScenes;
        public string[] UnloadScenes;
        
        public static IPromise Load(string[] loadScenes, SignalBus signalBus)
        {
            LoadUnloadScenesSignal loadUnloadParams = new LoadUnloadScenesSignal()
            {
                LoadScenes = loadScenes,
                UnloadScenes = null
            };

            signalBus.Fire(loadUnloadParams);

            return loadUnloadParams.Fire();
        }

        public static IPromise Load(string loadScene, SignalBus signalBus)
        {
            LoadUnloadScenesSignal loadUnloadParams = new LoadUnloadScenesSignal()
            {
                LoadScenes = new[] { loadScene },
                UnloadScenes = null
            };

            signalBus.Fire(loadUnloadParams);

            return loadUnloadParams.Fire();
        }

        public static IPromise Unload(string[] unloadScenes, SignalBus signalBus)
        {
            LoadUnloadScenesSignal loadUnloadParams = new LoadUnloadScenesSignal()
            {
                LoadScenes = null,
                UnloadScenes = unloadScenes
            };

            signalBus.Fire(loadUnloadParams);

            return loadUnloadParams.Fire();
        }

        public static IPromise Unload(string unloadScene, SignalBus signalBus)
        {
            LoadUnloadScenesSignal loadUnloadParams = new LoadUnloadScenesSignal()
            {
                LoadScenes = null,
                UnloadScenes = new[] { unloadScene }
            };

            signalBus.Fire(loadUnloadParams);

            return loadUnloadParams.Fire();
        }

        public static IPromise LoadUnload(string[] loadScenes, string[] unloadScenes, SignalBus signalBus)
        {
            LoadUnloadScenesSignal loadUnloadParams = new LoadUnloadScenesSignal()
            {
                LoadScenes = loadScenes,
                UnloadScenes = unloadScenes
            };

            signalBus.Fire(loadUnloadParams);

            return loadUnloadParams.Fire();
        }

        public static IPromise LoadUnload(string loadScene, string unloadScene, SignalBus signalBus)
        {
            LoadUnloadScenesSignal loadUnloadParams = new LoadUnloadScenesSignal()
            {
                LoadScenes = new[] { loadScene },
                UnloadScenes = new[] { unloadScene }
            };

            signalBus.Fire(loadUnloadParams);

            return loadUnloadParams.Fire();
        }
    }

    public class UnloadSceneSignal : ASignal
    {
        public string Scene;
        
        public static IPromise Unload(string scene, SignalBus signalBus)
        {
            UnloadSceneSignal unloadParams = new UnloadSceneSignal()
            {
                Scene = scene
            };

            signalBus.Fire(unloadParams);

            return unloadParams.Fire();
        }
    }

    public class UnloadAllScenesExceptSignal : ASignal
    {
        public string Scene;
        
        public static IPromise UnloadAllExcept(string scene, SignalBus signalBus)
        {
            UnloadAllScenesExceptSignal unloadParams = new UnloadAllScenesExceptSignal()
            {
                Scene = scene
            };

            signalBus.Fire(unloadParams);

            return unloadParams.Fire();
        }
    }
}