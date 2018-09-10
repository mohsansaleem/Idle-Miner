using RSG;

namespace pg.core.view
{
    public class OpenPopupSignalParams
    {
        public IPopupConfig PopupConfig;
        public Promise<IPopupResult> OnPopupComplete;
    }
}