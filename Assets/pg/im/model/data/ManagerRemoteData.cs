using Newtonsoft.Json;
using System;

namespace pg.im.model.data
{
    [Serializable]
    public class ManagerRemoteData
    {
        [JsonProperty("Id")]
        public string ManagerId;

        [JsonProperty("PurchaseTime")]
        public DateTime PurchaseTime;
    }
}