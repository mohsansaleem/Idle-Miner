using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}