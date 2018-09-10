using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace pg.im.model.data
{
    [Serializable]
    public class ManagersRemoteData
    {
        [JsonProperty("MinningManagers")]
        public List<ManagerRemoteData> MinningManagers;

        [JsonProperty("ElevatorManagers")]
        public List<ManagerRemoteData> ElevatorManagers;

        [JsonProperty("WareHouseManagers")]
        public List<ManagerRemoteData> WareHouseManagers;
    }
}