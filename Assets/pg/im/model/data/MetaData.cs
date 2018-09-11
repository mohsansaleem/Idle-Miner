using Newtonsoft.Json;
using System.Collections.Generic;

namespace pg.im.model.data
{
    public class MetaData
    {
        [JsonProperty("Shafts")]
        public Dictionary<string,List<ShaftLevelData>> Shafts;

        [JsonProperty("Elevator")]
        public List<ElevatorLevelData> Elevator;

        [JsonProperty("Managers")]
        public ManagersData Managers;

        [JsonProperty("Warehouse")]
        public List<WarehouseLevelData> Warehouse;

        [JsonProperty("MineLength")]
        public int MineLength;

        [JsonProperty("ShaftDistance")]
        public int ShaftDepth;

        [JsonProperty("WarehouseDistance")]
        public int WarehouseDistance;
    }
}