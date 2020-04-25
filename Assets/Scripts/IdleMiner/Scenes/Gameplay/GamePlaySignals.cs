using Zenject;

namespace PG.IdleMiner.Scenes.Gameplay
{
    public class AddShaftSignal : Signal<AddShaftSignal> { }
    
    public class UpgradeElevatorSignal : Signal<UpgradeElevatorSignal> { }
    public class UpgradeWarehouseSignal : Signal<UpgradeElevatorSignal> { }
}