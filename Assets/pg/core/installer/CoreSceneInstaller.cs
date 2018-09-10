using UnityEngine;
using Zenject;
using pg.core.view;
using RSG;
using System;

namespace pg.core.installer
{
    public enum SceneType
    {
        Screen = 0,
        Popup = 1,
        HUD = 2,
        Loader = 3
    }

    public class CoreSceneInstaller : MonoInstaller
    {
        [SerializeField] private Camera[] SceneCameras;
        [SerializeField] private Canvas[] SceneCanvases;

        [SerializeField] public SceneType SceneType = SceneType.Screen;

        //Max scene cache time (> 0)
        //No scene caching (= 0)
        //Infinite cache time (< 0)
        [SerializeField] public int MaxCacheSeconds = 60;

        //How heavy this scene is considered when cleaning the cache
        [SerializeField] public float CacheWeight = 10f;


        private Type _lastValidOpenState = null;
        public Type LastValidOpenState { get { return _lastValidOpenState; } }

        private BaseSceneParams _openParams = null;
        public BaseSceneParams OpenParams { get { return _openParams; } }

        [Inject] private readonly CoreSceneManager _coreSceneManager;
        [Inject] private readonly RequestStateChangeSignal _requestStateChangeSignal;
        [Inject] private readonly SendSceneParamsSignal _sendSceneParamsSignal;

        public override void Start()
        {
            base.Start();
			if (_coreSceneManager != null)
			{
				_coreSceneManager.RegisterSceneInstaller(this);
			}
        }

        public override void InstallBindings()
        {
            Container.Bind<CoreSceneInstaller>().FromInstance(this);
        }

        public void SetSceneOrderDepth(int depth)
        {
            int i;
            if(SceneCameras != null)
            {
                for (i = 0; i < SceneCameras.Length; i++)
                {
                    SceneCameras[i].depth = (float)(depth + i) / 100f;
                }
            }
            if(SceneCanvases != null)
            {
                for (i = 0; i < SceneCanvases.Length; i++)
                {
                    SceneCanvases[i].sortingOrder = depth + i;
                }
            }
        }

        public void GoToState(Type openState, BaseSceneParams openParams)
        {
            if (openState == null)
            {
                openState = GetDefaultState();
                if (openState == null)
                {
                    _lastValidOpenState = null;

                    _openParams = openParams;
                    _sendSceneParamsSignal.Fire(openParams);
                    return;
                }
            }
            _lastValidOpenState = openState;
            _requestStateChangeSignal.Fire(openState);

            _openParams = openParams;
            _sendSceneParamsSignal.Fire(openParams);
        }

        public void OnNewValidOpenState(Type openState)
        {
            _lastValidOpenState = openState;
        }

        public virtual Type GetDefaultState()
        {
            return null;
        }

        public virtual IPromise Open()
        {
            Promise promise = new Promise();
            promise.Resolve();
            return promise;
        }

        public virtual IPromise Close()
        {
            Promise promise = new Promise();
            promise.Resolve();
            return promise;
        }

        public virtual void OnSceneWake()
        {

        }

        public virtual void OnSceneSleep()
        {
            
        }
    }
}
