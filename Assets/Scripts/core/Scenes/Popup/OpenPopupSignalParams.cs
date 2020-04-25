using RSG;

namespace PG.Core.Scenes.Popup
{
    public class OpenPopupSignalParams
    {
        public IPopupConfig PopupConfig;
        public Promise<IPopupResult> OnPopupComplete;
    }
}