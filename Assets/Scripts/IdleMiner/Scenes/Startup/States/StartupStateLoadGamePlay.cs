using PG.Core.Installers;
using PG.IdleMiner.Models.MediatorModels;

namespace PG.IdleMiner.Scenes.Startup
{
    public partial class StartupMediator
    {
        public class StartupStateLoadGamePlay : StartupState
        {
            private readonly LoadUnloadScenesSignal _loadUnloadScenesSignal;

            public StartupStateLoadGamePlay(StartupMediator mediator):base(mediator)
            {
                _loadUnloadScenesSignal = Mediator._loadUnloadScenesSignal;
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                _loadUnloadScenesSignal.Load(new[] { ProjectScenes.Game }).Done
                (
                    () =>
                    {
                        StartupModel.LoadingProgress.Value = StartupModel.ELoadingProgress.GamePlayLoaded;
                    }
                );
            }
        }
    }
}