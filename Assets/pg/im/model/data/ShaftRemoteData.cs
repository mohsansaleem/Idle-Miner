
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace pg.im.model.data
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
    }
}