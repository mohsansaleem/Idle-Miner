using pg.core.installer;
using RSG;
using UnityEngine;
using Zenject;

namespace pg.core.command
{
    public class LoadUnloadScenesCommand : BaseCommand
    {
        [Inject] private readonly ISceneLoader _sceneLoader;

        //First loads an optional array of scenes, then 
        //unloads a separate optional array of scenes.
        //Each call to load/unload is added into a 
        //promise chain. When complete, the last promise 
        //fires an optional OnComplete delegate.
        public void Execute(LoadUnloadScenesCommandParams loadUnloadParams)
        {
            IPromise lastPromise = null;

            //Load scenes
            if (loadUnloadParams.LoadScenes != null)
            {
                for (int i = 0; i < loadUnloadParams.LoadScenes.Length; i++)
                {
                    string sceneName = loadUnloadParams.LoadScenes[i];
                    if (lastPromise != null)
                    {
                        lastPromise = lastPromise.Then(() => _sceneLoader.LoadScene(sceneName));
                    }
                    else
                    {
                        lastPromise = _sceneLoader.LoadScene(sceneName);
                    }
                }
            }

            //Unload scenes
            if (loadUnloadParams.UnloadScenes != null)
            {
                for (int i = 0; i < loadUnloadParams.UnloadScenes.Length; i++)
                {
                    string sceneName = loadUnloadParams.UnloadScenes[i];
                    if (lastPromise != null)
                    {
                        lastPromise = lastPromise.Then(() => _sceneLoader.UnloadScene(sceneName));
                    }
                    else
                    {
                        lastPromise = _sceneLoader.UnloadScene(sceneName);
                    }
                }
            }

            //Add promise to resolve OnComplete
            if (lastPromise != null)
            {
                lastPromise.Done(
                    () =>
                    {
                        Debug.Log(string.Format("{0} , scene loading/unloading completed!", this));

                        if (loadUnloadParams.OnComplete != null)
                        {
                            loadUnloadParams.OnComplete.Resolve();
                        }
                    },
                    exception => 
                    {
                        if (loadUnloadParams.OnComplete != null)
                        {
                            loadUnloadParams.OnComplete.Reject(exception);
                        }
                    }
                );
            }
            else
            {
                Debug.Log(string.Format("{0} , no scenes loaded/unloaded!", this));

                if (loadUnloadParams.OnComplete != null)
                {
                    loadUnloadParams.OnComplete.Resolve();
                }
            }
        }
    }
}

