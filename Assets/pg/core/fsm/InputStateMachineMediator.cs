using System;
using System.Collections.Generic;
using pg.core.fsm;
using UnityEngine;
using UnityEngine.EventSystems;

namespace pg.core
{
    public class InputStateMachineMediator : StateMachineMediator, IInputTriggerHandler
    {
        private IInputTriggerHandler _currentInputStateBehaviour;
        protected Dictionary<EventTriggerType, Action<InputTriggerHandlerParams>> _inputTriggerCallbacks;

        public InputStateMachineMediator()
        {
            _inputTriggerCallbacks = new Dictionary<EventTriggerType, Action<InputTriggerHandlerParams>>()
            {
                { EventTriggerType.PointerDown, OnPointerDown },
                { EventTriggerType.PointerUp, OnPointerUp },
                { EventTriggerType.Drop, OnDrop },
                { EventTriggerType.PointerEnter, OnPointerEnter },
                { EventTriggerType.PointerExit, OnPointerExit },
                { EventTriggerType.BeginDrag, OnBeginDrag },
                { EventTriggerType.Drag, OnDrag },
                { EventTriggerType.EndDrag, OnEndDrag },
                { EventTriggerType.PointerClick, OnPointerClick }
            };
        }

        public override void GoToState(Type stateType)
        {
            if (_stateBehaviours.ContainsKey(stateType))
            {
                if (_currentStateBehaviour != null)
                {
                    _currentStateBehaviour.OnStateExit();
                }
                _currentStateBehaviour = _stateBehaviours[stateType];
                _currentStateBehaviour.OnStateEnter();
                _currentInputStateBehaviour = (InputStateBehaviour) _currentStateBehaviour;
            }
            else
            {
                Debug.LogError(string.Format("{0} : GoToState( {1} ) type does not exist in state machine", this, stateType));
            }
        }

        public virtual void OnBeginDrag(InputTriggerHandlerParams inputTriggerHandlerParams)
        {
            if (_currentInputStateBehaviour != null)
            {
                _currentInputStateBehaviour.OnBeginDrag(inputTriggerHandlerParams);
            }
        }

        public virtual void OnDrag(InputTriggerHandlerParams inputTriggerHandlerParams)
        {
            if (_currentInputStateBehaviour != null)
            {
                _currentInputStateBehaviour.OnDrag(inputTriggerHandlerParams);
            }
        }

        public virtual void OnDrop(InputTriggerHandlerParams inputTriggerHandlerParams)
        {
            if (_currentInputStateBehaviour != null)
            {
                _currentInputStateBehaviour.OnDrop(inputTriggerHandlerParams);
            }
        }

        public virtual void OnEndDrag(InputTriggerHandlerParams inputTriggerHandlerParams)
        {
            if (_currentInputStateBehaviour != null)
            {
                _currentInputStateBehaviour.OnEndDrag(inputTriggerHandlerParams);
            }
        }

        public virtual void OnPointerDown(InputTriggerHandlerParams inputTriggerHandlerParams)
        {
            if (_currentInputStateBehaviour != null)
            {
                _currentInputStateBehaviour.OnPointerDown(inputTriggerHandlerParams);
            }
        }

        public virtual void OnPointerEnter(InputTriggerHandlerParams inputTriggerHandlerParams)
        {
            if (_currentInputStateBehaviour != null)
            {
                _currentInputStateBehaviour.OnPointerEnter(inputTriggerHandlerParams);
            }
        }

        public virtual void OnPointerExit(InputTriggerHandlerParams inputTriggerHandlerParams)
        {
            if (_currentInputStateBehaviour != null)
            {
                _currentInputStateBehaviour.OnPointerExit(inputTriggerHandlerParams);
            }
        }

        public virtual void OnPointerUp(InputTriggerHandlerParams inputTriggerHandlerParams)
        {
            if (_currentInputStateBehaviour != null)
            {
                _currentInputStateBehaviour.OnPointerUp(inputTriggerHandlerParams);
            }
        }

        public virtual void OnPointerClick(InputTriggerHandlerParams inputTriggerHandlerParams)
        {
            if (_currentInputStateBehaviour != null)
            {
                _currentInputStateBehaviour.OnPointerClick(inputTriggerHandlerParams);
            }
        }
    }
}
