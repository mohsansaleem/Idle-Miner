using RSG;

namespace PG.Core.Contexts.Popup
{
    public class OpenPopupSignalParams
    {
        public IPopupConfig PopupConfig;
        public Promise<IPopupResult> OnPopupComplete;
    }
}