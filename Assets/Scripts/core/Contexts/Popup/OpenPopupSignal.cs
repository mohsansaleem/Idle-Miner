using RSG;
using Zenject;

namespace PG.Core.Contexts.Popup
{
    public class OpenPopupSignal : Signal<OpenPopupSignalParams, OpenPopupSignal>
    {
        public Promise<IPopupResult> ShowPopup(IPopupConfig popupConfig)
        {
            OpenPopupSignalParams openPopupParams =
                new OpenPopupSignalParams
                {
                    OnPopupComplete = new Promise<IPopupResult>(),
                    PopupConfig = popupConfig
                };

            Fire(openPopupParams);

            return openPopupParams.OnPopupComplete;
        }
    }
}