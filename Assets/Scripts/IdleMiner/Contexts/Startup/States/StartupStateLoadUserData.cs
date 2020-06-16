using PG.IdleMiner.Models.MediatorModels;
using RSG;

namespace PG.IdleMiner.Contexts.Startup
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