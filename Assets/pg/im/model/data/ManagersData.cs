using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace pg.im.model.data
{
    [Serializable]
    public class ManagersData
    {
        [JsonProperty("MinningManagers")]
        public List<ManagerData> MinningManagers;

        [JsonProperty("ElevatorManagers")]
        public List<ManagerData> ElevatorManagers;

        [JsonProperty("WareHouseManagers")]
        public List<ManagerData> WareHouseManagers;
    }
}