using PG.Core.Contexts;
using PG.IdleMiner.Models.MediatorModels;
using PG.IdleMiner.Views.Hud;

namespace PG.IdleMiner.view
{
    public partial class HudMediator
    {
        public class HudState : StateBehaviour
        {
            protected readonly HudMediator Mediator;
            protected readonly HudModel HudModel;
            protected readonly HudView View;

            public HudState(HudMediator mediator)
            {
                Mediator = mediator;
                HudModel = mediator._hudModel;
                View = mediator._view;
            }
        }
    }
}
