using PG.Core.Contexts;
using PG.IdleMiner.Models.MediatorModels;
using PG.IdleMiner.Views.Startup;

namespace PG.IdleMiner.Contexts.Startup
{
    public partial class StartupMediator
    {
        public class StartupState : StateBehaviour
        {
            protected readonly StartupMediator Mediator;
            protected readonly StartupModel StartupModel;
            protected readonly StartupView View;

            public StartupState(StartupMediator mediator) : base(mediator)
            {
                Mediator = mediator;
                StartupModel = mediator._startupModel;
                View = mediator._view;
            }
        }
    }
}
