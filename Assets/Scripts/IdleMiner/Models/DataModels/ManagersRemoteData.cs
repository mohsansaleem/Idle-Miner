using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PG.IdleMiner.Models.DataModels
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