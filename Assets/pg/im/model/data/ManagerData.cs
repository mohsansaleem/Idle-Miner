using Newtonsoft.Json;
using System;

namespace pg.im.model.data
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