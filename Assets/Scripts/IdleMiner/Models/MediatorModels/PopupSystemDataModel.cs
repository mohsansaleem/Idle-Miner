using PG.IdleMiner.view.popup;
using UniRx;

namespace PG.IdleMiner.Models.MediatorModels
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

