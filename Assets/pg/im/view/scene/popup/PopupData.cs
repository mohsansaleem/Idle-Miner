using pg.core.view;
using pg.im.view.popup.popupconfig;
using RSG;

namespace pg.im.view.popup
{
    public class PopupData
    {
        public PopupConfig PopupConfig;
        public Promise<IPopupResult> OnPopupComplete;
    }
}