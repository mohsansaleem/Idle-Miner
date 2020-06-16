namespace PG.IdleMiner.Contexts.GamePlay
{
    public partial class GamePlayMediator
    {
        public class GamePlayStateDefault : GamePlayState
        {
            public GamePlayStateDefault(GamePlayMediator mediator):base(mediator)
            {
            }

            public override void OnStateEnter()
            {
                base.OnStateEnter();

                View.AddShaftButton.onClick.AddListener(OnAddShaftButtonClicked);
            }

            private void OnAddShaftButtonClicked()
            {
                SignalBus.Fire<AddShaftSignal>();
            }

            public override void OnStateExit()
            {
                base.OnStateExit();

                View.AddShaftButton.onClick.RemoveListener(OnAddShaftButtonClicked);
            }
        }
    }
}