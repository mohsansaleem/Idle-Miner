
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace pg.im.model.data
{
    [Serializable]
    public class WarehouseRemoteData
    {
        [JsonProperty("Level")]
        public int WarehouseLevel;

        [JsonProperty("Manager")]
        public string Manager;

        [JsonProperty("Transporters")]
        public List<TransporterRemoteData> Transporters;

        [JsonIgnore]
        public WarehouseLevelData WarehouseLevelData { get; set; }
    }
}