using UniRx;

namespace PG.IdleMiner.Models.MediatorModels
{
    public class HudModel
    {
        public enum EHudState
        {
            StartupScreen,
            GamePlay,
            UpgradeScreen
        }

        public ReactiveProperty<EHudState> HudState;

        public HudModel()
        {
            HudState = new ReactiveProperty<EHudState>(EHudState.StartupScreen);
        }
    }
}

