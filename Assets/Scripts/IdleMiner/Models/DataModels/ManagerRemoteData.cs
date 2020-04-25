using System;
using Newtonsoft.Json;

namespace PG.IdleMiner.Models.DataModels
{
    [Serializable]
    public class ManagerRemoteData
    {
        [JsonProperty("Id")]
        public string ManagerId;

        [JsonProperty("PurchaseTime")]
        public DateTime PurchaseTime;

        [JsonIgnore]
        public ManagerData ManagerData { get; set; }
    }
}