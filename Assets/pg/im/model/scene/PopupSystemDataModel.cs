using pg.im.view.popup;
using UniRx;

namespace pg.im.model.scene
{
    public class PopupSystemDataModel
    {
        public ReactiveCollection<PopupData> Popups;

        public PopupSystemDataModel()
        {
            Popups = new ReactiveCollection<PopupData>();
        }
    }
}

