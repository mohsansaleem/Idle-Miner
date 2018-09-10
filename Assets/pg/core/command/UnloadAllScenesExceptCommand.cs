using pg.core.installer;
using RSG;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace pg.core.command
{
    public class UnloadAllScenesExceptCommand : BaseCommand
    {
        [Inject] private readonly ISceneLoader _sceneLoader;

        public void Execute(LoadSceneCommandParams loadParams)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadParams.Scene));

            IPromise lastPromise = null;

            int count = SceneManager.sceneCount;

            for (int i = 0; i < count; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                if (scene.isLoaded && !scene.name.Equals(loadParams.Scene))
                {
                    if (lastPromise != null)
                    {
                        lastPromise = lastPromise.Then(() => _sceneLoader.UnloadScene(scene.name));
                    }
                    else
                    {
                        lastPromise = _sceneLoader.UnloadScene(scene.name);
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

                        if (loadParams.OnComplete != null)
                        {
                            loadParams.OnComplete.Resolve();
                        }
                    },
                    exception =>
                    {
                        Debug.LogError("UnloadAllScenesExceptCommand.Execute: " + exception.ToString());

                        if (loadParams.OnComplete != null)
                        {
                            loadParams.OnComplete.Reject(exception);
                        }
                    }
                );
            }
            else
            {
                Debug.Log(string.Format("{0} , no scenes loaded/unloaded!", this));

                if (loadParams.OnComplete != null)
                {
                    loadParams.OnComplete.Resolve();
                }
            }
        }
    }
}
