using pg.core.installer;
using pg.im.view.scene;
using pg.im.model;

namespace pg.im.view
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

                _loadUnloadScenesSignal.Load(new[] { Scenes.Game }).Done
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