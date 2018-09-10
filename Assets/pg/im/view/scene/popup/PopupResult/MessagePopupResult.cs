using pg.core.view;

namespace pg.im.view.popup.popupresult
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