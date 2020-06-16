using PG.IdleMiner.Models.MediatorModels;
using UnityEngine;
using RSG;

namespace PG.IdleMiner.Contexts.Startup
{
    public partial class StartupMediator
    {
        public class StartupStateLoadStaticData : StartupState
        {
            public StartupStateLoadStaticData(StartupMediator mediator) : base(mediator)
            {
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                LoadStaticDataSignal signal = new LoadStaticDataSignal() {Promise = new Promise()};

                signal.Promise.Then(
                    () => { StartupModel.LoadingProgress.Value = StartupModel.ELoadingProgress.LoadUserData; }
                ).Catch(e => { Debug.LogError("Exception seeding static data 2: " + e.ToString()); });

                SignalBus.Fire(signal);
            }
        }
    }
}