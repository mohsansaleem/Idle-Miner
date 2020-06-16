using PG.Core.Contexts.Popup;

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