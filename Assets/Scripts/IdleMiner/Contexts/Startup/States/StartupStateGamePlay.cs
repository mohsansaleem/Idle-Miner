using System;
using PG.IdleMiner.Misc;
using UniRx;

namespace PG.IdleMiner.Contexts.Startup
{
    public partial class StartupMediator
    {
        public class StartupStateGamePlay : StartupState
        {
            private readonly SaveUserDataSignal _saveUserDataSignal;

            public StartupStateGamePlay(StartupMediator mediator):base(mediator)
            {
                _saveUserDataSignal = mediator._saveUserDataSignal;
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                View.Hide();

                Observable.Timer(TimeSpan.FromSeconds(Constants.SaveGameDelay)).Repeat().Subscribe((interval) => _saveUserDataSignal.Fire()).AddTo(_disposables);
            }



            public override void OnStateExit()
            {
                base.OnStateExit();
            }
        }
    }
}