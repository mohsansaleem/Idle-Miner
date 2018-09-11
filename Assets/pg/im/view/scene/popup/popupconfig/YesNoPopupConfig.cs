using pg.core.view;
using pg.im.view.popup.popupresult;

namespace pg.im.view.popup.popupconfig
{
    public class YesNoPopupConfig : PopupConfig
    {
        public static IPopupConfig GetYesNoPopupConfig(string title, string question)
        {
            // @todo - MS - Localization. 
            return PopulatedConfigInstance(new YesNoPopupConfig(), title, question, "Yes", "No");
        }

        public override IPopupResult GetPopupResult()
        {
            return new YesNoPopupResult();
        }
    }
}