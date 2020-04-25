using System;
using Newtonsoft.Json;

namespace PG.IdleMiner.Models.DataModels
{
    [Serializable]
    public class ManagerData
    {
        [JsonProperty("Id")]
        public string ManagerId;

        [JsonProperty("Name")]
        public string ManagerName;

        [JsonProperty("HiringCost")]
        public double HiringCost;
    }
}