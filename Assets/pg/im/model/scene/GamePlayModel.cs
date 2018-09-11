using UniRx;

namespace pg.im.model.scene
{
    public class GamePlayModel
    {
        public enum EGamePlayState
        {
            Game,
            Overlay
        }

        public ReactiveProperty<EGamePlayState> GamePlayState;

        public GamePlayModel()
        {
            GamePlayState = new ReactiveProperty<EGamePlayState>(EGamePlayState.Game);
        }
    }
}

