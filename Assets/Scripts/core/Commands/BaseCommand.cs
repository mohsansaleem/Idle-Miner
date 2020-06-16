using System;
using System.Collections.Generic;
using PG.Core.Contexts.Popup;
using RSG;
using Zenject;

namespace PG.Core.Commands
{
    public abstract class BaseCommand
    {
        [Inject] protected SignalBus SignalBus;
    }
}