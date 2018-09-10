using System;
using System.Collections.Generic;
using pg.core.view;
using pg.core.installer;
using Zenject;
using RSG;
using UniRx;

namespace pg.core
{
    public class StateMachineMediator : IInitializable, ITickable, IDisposable
    {
        protected StateBehaviour _currentStateBehaviour;
        protected Dictionary<Type, StateBehaviour> _stateBehaviours = new Dictionary<Type, StateBehaviour>();


        protected CompositeDisposable _disposables;

        [Inject] protected CoreSceneManager _sceneManager;
        public CoreSceneManager SceneManager { get { return _sceneManager; } }

        [Inject] protected CoreSceneInstaller _sceneInstaller;
        [Inject] protected OpenPopupSignal _openPopupSignal;
        [Inject] protected RequestStateChangeSignal _requestStateChangeSignal;
        [Inject] protected SendSceneParamsSignal _sendSceneParamsSignal;

        public virtual void Initialize()
		{
			_disposables = new CompositeDisposable();
            _requestStateChangeSignal.Listen(GoToState);
		    _sendSceneParamsSignal.Listen(ReceiveSceneParams);
        }

        public virtual void ReceiveSceneParams(BaseSceneParams sceneParams)
        {
            /*
             * This is called from a signal whenever a new scene is opened by
             * the scene manager. This allows some custom parameters to be
             * passed when opening the scene (i.e. PlayerProfile receives playerId)
             */

            //Do nothing in virtual method
        }

        public virtual void GoToState(Type stateType)
        {
            if (_stateBehaviours.ContainsKey(stateType))
            {
                if (_currentStateBehaviour != null)
                {
                    _currentStateBehaviour.OnStateExit();
                }
                _currentStateBehaviour = _stateBehaviours[stateType];
                if (_sceneInstaller != null && _currentStateBehaviour.IsValidOpenState())
                {
                    _sceneInstaller.OnNewValidOpenState(stateType);
                }
                _currentStateBehaviour.OnStateEnter();
            }
        }

        public virtual Promise<IPopupResult> ShowPopup(IPopupConfig popupConfig)
        {
            return _openPopupSignal.ShowPopup(popupConfig);
        }

        public virtual void Tick()
        {
            if (_currentStateBehaviour != null)
            {
                _currentStateBehaviour.Tick();
            }
        }

        public virtual void Dispose()
        {
            _requestStateChangeSignal.Unlisten(GoToState);

            if (_currentStateBehaviour != null)
            {
                _currentStateBehaviour.OnStateExit();
            }

            _disposables.Dispose();

            _stateBehaviours.Clear();
        }
    }
}
