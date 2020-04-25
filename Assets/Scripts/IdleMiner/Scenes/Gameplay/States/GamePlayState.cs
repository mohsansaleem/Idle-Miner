using PG.Core.FSM;
using PG.IdleMiner.Models.MediatorModels;

namespace PG.IdleMiner.Scenes.Gameplay
{
    public partial class GamePlayMediator
    {
        public class GamePlayState : StateBehaviour
        {
            protected readonly GamePlayMediator Mediator;
            protected readonly GamePlayModel GamePlayModel;
            protected readonly GamePlayView View;

            public GamePlayState(GamePlayMediator mediator)
            {
                Mediator = mediator;
                GamePlayModel = mediator._gamePlayModel;
                View = mediator._view;
            }
        }
    }
}
