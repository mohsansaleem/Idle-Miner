using UniRx;

namespace pg.im.model.scene
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

