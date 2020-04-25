using PG.Core.FSM;
using PG.IdleMiner.Models.MediatorModels;

namespace PG.IdleMiner.Scenes.Startup
{
    public partial class StartupMediator
    {
        public class StartupState : StateBehaviour
        {
            protected readonly StartupMediator Mediator;
            protected readonly StartupModel StartupModel;
            protected readonly StartupView View;

            public StartupState(StartupMediator mediator)
            {
                Mediator = mediator;
                StartupModel = mediator._startupModel;
                View = mediator._view;
            }
        }
    }
}
