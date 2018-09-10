using pg.core;
using pg.im.model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pg.im.view
{
    public partial class HudMediator
    {
        public class HudState : StateBehaviour
        {
            protected readonly HudMediator Mediator;
            protected readonly HudModel HudModel;
            protected readonly HudView View;

            public HudState(HudMediator mediator)
            {
                this.Mediator = mediator;
                this.HudModel = mediator._hudModel;
                this.View = mediator._view;
            }
        }
    }
}
