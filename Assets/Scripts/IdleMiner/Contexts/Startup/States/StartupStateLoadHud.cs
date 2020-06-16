using PG.Core.Installers;
using PG.IdleMiner.Models.MediatorModels;

namespace PG.IdleMiner.Contexts.Startup
{
    public partial class StartupMediator
    {
        public class StartupStateLoadHud : StartupState
        {
            public StartupStateLoadHud(StartupMediator mediator) : base(mediator)
            {
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                LoadUnloadScenesSignal.Load(ProjectScenes.Hud, SignalBus).Done
                (
                    () => { StartupModel.LoadingProgress.Value = StartupModel.ELoadingProgress.LoadGamePlay; },
                    exception => { }
                );
            }
        }
    }
}