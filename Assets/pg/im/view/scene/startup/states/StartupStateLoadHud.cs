using pg.core.installer;
using pg.im.view.scene;
using pg.im.model.scene;

namespace pg.im.view
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

                _loadUnloadScenesSignal.Load(Scenes.Hud).Done
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