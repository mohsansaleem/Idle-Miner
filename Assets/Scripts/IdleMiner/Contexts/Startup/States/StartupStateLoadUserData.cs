using PG.IdleMiner.Models.MediatorModels;
using RSG;

namespace PG.IdleMiner.Contexts.Startup
{
    public partial class StartupMediator
    {
        public class StartupStateLoadUserData : StartupState
        {
            public StartupStateLoadUserData(StartupMediator mediator) : base(mediator)
            {
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                LoadUserDataSignal signal = new LoadUserDataSignal() {Promise = new Promise()};

                signal.Promise.Then(
                    () => { StartupModel.LoadingProgress.Value = StartupModel.ELoadingProgress.LoadHud; }
                ).Catch(e => { StartupModel.LoadingProgress.Value = StartupModel.ELoadingProgress.CreateUserData; });

                SignalBus.Fire(signal);
            }
        }
    }
}