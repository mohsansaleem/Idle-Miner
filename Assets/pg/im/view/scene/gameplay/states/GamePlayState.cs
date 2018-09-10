using pg.core;
using pg.im.model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pg.im.view
{
    public partial class GamePlayMediator
    {
        public class GamePlayState : StateBehaviour
        {
            protected readonly GamePlayMediator Mediator;
            protected readonly GamePlayModel GamePlayModel;
            protected readonly GamePlayView View;

            public GamePlayState(GamePlayMediator mediator)
            {
                this.Mediator = mediator;
                this.GamePlayModel = mediator._gamePlayModel;
                this.View = mediator._view;
            }
        }
    }
}
