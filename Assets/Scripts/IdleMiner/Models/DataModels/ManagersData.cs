using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PG.IdleMiner.Models.DataModels
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