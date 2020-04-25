using PG.Core.Scenes.Popup;

namespace PG.IdleMiner.view.popup.popupresult
{
    public class MessagePopupResult : PopupResult
    {
        public bool Ok
        {
            get
            {
                return SelectedIndex == 0;
            }
        }
    }
}