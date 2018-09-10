using pg.core.view;

namespace pg.im.view.popup.popupresult
{
    public class OkCancelPopupResult : PopupResult
    {
        public bool Ok
        {
            get
            {
                return SelectedIndex == 0;
            }
        }

        public bool Cancel
        {
            get
            {
                return SelectedIndex == 1;
            }
        }
    }
}