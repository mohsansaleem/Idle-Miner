using PG.Core.Installers;
using RSG;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace PG.Core.Commands
{
    public class UnloadAllScenesExceptCommand : BaseCommand
    {
        [Inject] private readonly ISceneLoader _sceneLoader;

        public void Execute(UnloadAllScenesExceptSignal loadParams)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadParams.Scene));

            IPromise lastPromise = null;

            int count = SceneManager.sceneCount;

            for (int i = 0; i < count; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                if (scene.isLoaded && !scene.name.Equals(loadParams.Scene))
                {
                    lastPromise = lastPromise != null ? lastPromise.Then(() => _sceneLoader.UnloadScene(scene.name)) : _sceneLoader.UnloadScene(scene.name);
                }
            }

            //Add promise to resolve OnComplete
            if (lastPromise != null)
            {
                lastPromise.Done(
                    () =>
                    {
                        Debug.Log($"{this} , scene loading/unloading completed!");

                        loadParams.OnComplete?.Resolve();
                    },
                    exception =>
                    {
                        Debug.LogError("UnloadAllScenesExceptCommand.Execute: " + exception.ToString());

                        loadParams.OnComplete?.Reject(exception);
                    }
                );
            }
            else
            {
                Debug.Log($"{this} , no scenes loaded/unloaded!");

                loadParams.OnComplete?.Resolve();
            }
        }
    }
}
