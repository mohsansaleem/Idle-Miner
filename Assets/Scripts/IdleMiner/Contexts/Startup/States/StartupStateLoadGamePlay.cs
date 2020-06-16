using PG.Core.Installers;
using PG.IdleMiner.Models.MediatorModels;

namespace PG.IdleMiner.Contexts.Startup
{
    public partial class StartupMediator
    {
        public class StartupStateLoadGamePlay : StartupState
        {
            public StartupStateLoadGamePlay(StartupMediator mediator) : base(mediator)
            {
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                LoadUnloadScenesSignal.Load(new[] {ProjectScenes.Game}, SignalBus).Done
                (
                    () => { StartupModel.LoadingProgress.Value = StartupModel.ELoadingProgress.GamePlay; }
                );
            }
        }
    }
}