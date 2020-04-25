
namespace PG.IdleMiner.Misc
{
    public static class Constants
    {
        public const string MetaDataFile = "MetaData.json";
        public const string GameStateFile = "GameState.json";
        public const double SaveGameDelay = 5;

        // Keeping the constants here. No time to add a flow from Meta to Views. Proper way would have having the Size in Shaft, Elevator and Warehouse respective.
        public const int MineLength = 8;
        public const int WarehouseDistance = 10;
        public const int ShaftDistance = 4;
        
        public const float FacadesTickTime = 1f;
    }
}
