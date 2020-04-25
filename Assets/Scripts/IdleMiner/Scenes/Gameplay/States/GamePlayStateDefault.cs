namespace PG.IdleMiner.Scenes.Gameplay
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

                View.AddShaftButton.onClick.AddListener(OnAddShaftButtonClicked);
            }

            private void OnAddShaftButtonClicked()
            {
                _addShaftSignal.Fire();
            }

            public override void OnStateExit()
            {
                base.OnStateExit();

                View.AddShaftButton.onClick.RemoveListener(OnAddShaftButtonClicked);
            }
        }
    }
}