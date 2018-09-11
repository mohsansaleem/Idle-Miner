using System.Collections;
using RSG;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace pg.core
{
    public class AsyncSceneLoader : ISceneLoader
    {
        [Inject] private readonly CoroutineRunner _coroutineRunner;

        public IPromise LoadScene(string sceneName)
        {
            Promise loadPromise = new Promise();
            _coroutineRunner.StartCoroutine(AsynchronousLoad(sceneName, loadPromise));
            return loadPromise;
        }

        public IPromise UnloadScene(string sceneName)
        {
            Promise loadPromise = new Promise();
            _coroutineRunner.StartCoroutine(AsynchronousLoad(sceneName, loadPromise, true));
            return loadPromise;
        }

        IEnumerator AsynchronousLoad(string sceneName, Promise loadPromise, bool unload = false)
        {
            yield return null;

            AsyncOperation ao;
            if (!unload)
            {
                ao = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
            else
            {
                ao = SceneManager.UnloadSceneAsync(sceneName);
            }
            
            while (!ao.isDone)
            {
                // [0, 0.9] > [0, 1]
                float loadProgress = Mathf.Clamp01(ao.progress);
                loadPromise.ReportProgress(loadProgress);
                Debug.Log(string.Format("{0} , Async load progress = {1}", this, loadProgress));

                yield return null;
            }

            if (loadPromise != null)
            {
                Debug.Log(string.Format("{0} , Async load complete, completing promise", this));
                loadPromise.ReportProgress(1f);
                loadPromise.Resolve();
            }
        }

        public IPromise UnloadScene(Scene scene)
        {
            Promise loadPromise = new Promise();
            _coroutineRunner.StartCoroutine(AsynchronousUnload(scene, loadPromise));
            return loadPromise;
        }

        IEnumerator AsynchronousUnload(Scene scene, Promise loadPromise)
        {
            yield return null;

            AsyncOperation ao = SceneManager.UnloadSceneAsync(scene);

            while (!ao.isDone)
            {
                // [0, 0.9] > [0, 1]
                float loadProgress = Mathf.Clamp01(ao.progress);
                loadPromise.ReportProgress(loadProgress);
                Debug.Log(string.Format("{0} , Async load progress = {1}", this, loadProgress));

                yield return null;
            }

            if (loadPromise != null)
            {
                Debug.Log(string.Format("{0} , Async load complete, completing promise", this));
                loadPromise.ReportProgress(1f);
                loadPromise.Resolve();
            }
        }
    }
}
