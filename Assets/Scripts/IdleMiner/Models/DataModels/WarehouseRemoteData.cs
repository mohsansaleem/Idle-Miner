using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PG.IdleMiner.Models.DataModels
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