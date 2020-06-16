using PG.Core.Contexts;
using PG.IdleMiner.Models.MediatorModels;
using PG.IdleMiner.Views.GamePlay;
using Zenject;

namespace PG.IdleMiner.Contexts.GamePlay
{
    public partial class GamePlayMediator
    {
        public class GamePlayState : StateBehaviour
        {
            protected readonly GamePlayMediator Mediator;
            protected readonly GamePlayModel GamePlayModel;
            protected readonly GamePlayView View;

            public GamePlayState(GamePlayMediator mediator) : base(mediator)
            {
                Mediator = mediator;
                GamePlayModel = mediator._gamePlayModel;
                View = mediator._view;
            }
        }
    }
}