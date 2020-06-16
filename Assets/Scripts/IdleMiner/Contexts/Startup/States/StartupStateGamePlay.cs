using System;
using PG.IdleMiner.Misc;
using UniRx;

namespace PG.IdleMiner.Contexts.Startup
{
    public partial class StartupMediator
    {
        public class StartupStateGamePlay : StartupState
        {
            public StartupStateGamePlay(StartupMediator mediator) : base(mediator)
            {
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                View.Hide();

                Observable.Timer(TimeSpan.FromSeconds(Constants.SaveGameDelay)).Repeat()
                    .Subscribe((interval) => SignalBus.Fire<SaveUserDataSignal>()).AddTo(Disposables);
            }


            public override void OnStateExit()
            {
                base.OnStateExit();
            }
        }
    }
}