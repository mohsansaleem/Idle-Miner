using pg.im.installer;
using RSG;

using pg.im.model.scene;

namespace pg.im.view
{
    public partial class StartupMediator
    {
        public class StartupStateLoadUserData : StartupState
        {
            private readonly LoadUserDataSignal _loadUserDataSignal;

            public StartupStateLoadUserData(StartupMediator mediator) : base(mediator)
            {
                _loadUserDataSignal = mediator._loadUserDataSignal;
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                Promise UserDataPromise = new Promise();

                UserDataPromise.Then(
                    () => {
                        StartupModel.LoadingProgress.Value = StartupModel.ELoadingProgress.DataSeeded;
                    }
                ).Catch(e =>
                {
                    StartupModel.LoadingProgress.Value = StartupModel.ELoadingProgress.UserNotFound;
                });

                _loadUserDataSignal.Fire(UserDataPromise);
            }
        }
    }
}