using pg.core.fsm;

namespace pg.core
{
    public interface IInputTriggerHandler
    {
        void OnPointerDown(InputTriggerHandlerParams inputTriggerHandlerParams);

        void OnPointerUp(InputTriggerHandlerParams inputTriggerHandlerParams);

        void OnPointerClick(InputTriggerHandlerParams inputTriggerHandlerParams);

        void OnPointerEnter(InputTriggerHandlerParams inputTriggerHandlerParams);

        void OnPointerExit(InputTriggerHandlerParams inputTriggerHandlerParams);

        void OnBeginDrag(InputTriggerHandlerParams inputTriggerHandlerParams);

        void OnDrag(InputTriggerHandlerParams inputTriggerHandlerParams);

        void OnEndDrag(InputTriggerHandlerParams inputTriggerHandlerParams);

        void OnDrop(InputTriggerHandlerParams inputTriggerHandlerParams);
    }
}