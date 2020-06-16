using PG.Core.Installers;
using PG.IdleMiner.Models.MediatorModels;

namespace PG.IdleMiner.Contexts.Startup
{
    public partial class StartupMediator
    {
        public class StartupStateLoadHud : StartupState
        {
            private readonly LoadUnloadScenesSignal _loadUnloadScenesSignal;

            public StartupStateLoadHud(StartupMediator mediator):base(mediator)
            {
                _loadUnloadScenesSignal = Mediator._loadUnloadScenesSignal;
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                _loadUnloadScenesSignal.Load(ProjectScenes.Hud).Done
                (
                    () =>
                    {
                        StartupModel.LoadingProgress.Value = StartupModel.ELoadingProgress.HudLoaded;
                    },
                exception =>
                {

                }
                );
            }
        }
    }
}