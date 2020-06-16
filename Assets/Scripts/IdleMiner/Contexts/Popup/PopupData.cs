using PG.Core.Contexts.Popup;
using PG.IdleMiner.view.popup.popupconfig;
using RSG;

namespace PG.IdleMiner.view.popup
{
    public class PopupData
    {
        public PopupConfig PopupConfig;
        public Promise<IPopupResult> OnPopupComplete;
    }
}