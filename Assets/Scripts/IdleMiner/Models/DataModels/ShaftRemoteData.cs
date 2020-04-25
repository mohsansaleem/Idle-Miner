using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PG.IdleMiner.Models.DataModels
{
    [Serializable]
    public class ShaftRemoteData
    {
        [JsonProperty("Id")]
        public string ShaftId;
        
        [JsonProperty("Level")]
        public int ShaftLevel;

        [JsonProperty("Manager")]
        public string Manager;
        
        [JsonProperty("BinCash")]
        public double BinCash;

        [JsonProperty("Miners")]
        public List<MinerRemoteData> Miners;

        [JsonIgnore]
        public ShaftLevelData ShaftLevelData { get; set; }
    }
}