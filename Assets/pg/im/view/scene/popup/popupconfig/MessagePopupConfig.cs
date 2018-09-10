using System.Collections.Generic;
using pg.core.view;
using pg.im.view.popup.popupresult;

namespace pg.im.view.popup.popupconfig
{
    public class MessagePopupConfig : PopupConfig
    {
        public static IPopupConfig GetMessagePopupConfig(string title, string message)
        {
            // @todo - MS - Localization.
            return PopulatedConfigInstance(new MessagePopupConfig(), title, message, "Ok");
        }


        public override IPopupResult GetPopupResult()
        {
            return new MessagePopupResult();
        }
    }
}