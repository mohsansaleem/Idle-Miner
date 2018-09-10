using pg.core.installer;

namespace pg.im.view
{
    public partial class StartupMediator
    {
        public class StartupStateGamePlay : StartupState
        {
            public StartupStateGamePlay(StartupMediator mediator):base(mediator)
            {
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                View.Hide();
            }
        }
    }
}