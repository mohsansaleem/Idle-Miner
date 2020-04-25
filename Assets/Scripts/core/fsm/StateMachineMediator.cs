using System;
using System.Collections.Generic;
using PG.Core.Scenes.Popup;
using RSG;
using UniRx;
using Zenject;

namespace PG.Core.FSM
{
    public class StateMachineMediator : IInitializable, ITickable, IDisposable
    {
        protected StateBehaviour _currentStateBehaviour;
        protected Dictionary<Type, StateBehaviour> _stateBehaviours = new Dictionary<Type, StateBehaviour>();


        protected CompositeDisposable _disposables;

        [Inject] protected OpenPopupSignal _openPopupSignal;

        public virtual void Initialize()
		{
			_disposables = new CompositeDisposable();
        }

        public virtual void GoToState(Type stateType)
        {
            if (_stateBehaviours.ContainsKey(stateType)  && _currentStateBehaviour != _stateBehaviours[stateType])
            {
                if (_currentStateBehaviour != null)
                {
                    _currentStateBehaviour.OnStateExit();
                }
                _currentStateBehaviour = _stateBehaviours[stateType];
                
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
            if (_currentStateBehaviour != null)
            {
                _currentStateBehaviour.OnStateExit();
            }

            _disposables.Dispose();

            _stateBehaviours.Clear();
        }
    }
}
