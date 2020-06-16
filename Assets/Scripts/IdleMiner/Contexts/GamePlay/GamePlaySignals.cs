using Zenject;

namespace PG.IdleMiner.Contexts.GamePlay
{
    public class AddShaftSignal : Signal<AddShaftSignal> { }
    
    public class UpgradeElevatorSignal : Signal<UpgradeElevatorSignal> { }
    public class UpgradeWarehouseSignal : Signal<UpgradeElevatorSignal> { }
}