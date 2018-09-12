using pg.im.installer;

namespace pg.im.view
{
    public partial class GamePlayMediator
    {
        public class GamePlayStateDefault : GamePlayState
        {
            private readonly AddShaftSignal _addShaftSignal;
            public GamePlayStateDefault(GamePlayMediator mediator):base(mediator)
            {
                _addShaftSignal = mediator._addShaftSignal;
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                this.View.AddShaftButton.onClick.AddListener(OnAddShaftButtonClicked);
            }

            private void OnAddShaftButtonClicked()
            {
                _addShaftSignal.Fire();
            }

            public override void OnStateExit()
            {
                base.OnStateExit();

                this.View.AddShaftButton.onClick.RemoveListener(OnAddShaftButtonClicked);
            }
        }
    }
}