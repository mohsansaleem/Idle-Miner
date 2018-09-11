using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace pg.im.model.data
{
    [Serializable]
    public class UserData
    {
        [JsonProperty("Shafts")]
        public List<ShaftRemoteData> UserShafts;

        [JsonProperty("Elevator")]
        public ElevatorRemoteData Elevator;

        [JsonProperty("Warehouse")]
        public WarehouseRemoteData Warehouse;

        [JsonProperty("ManagersUnlocked")]
        public ManagersRemoteData Managers;

        [JsonProperty("IdleCash")]
        public double IdleCash;

        [JsonProperty("Cash")]
        public double Cash;

        [JsonProperty("SuperCash")]
        public double SuperCash;

        [JsonProperty("LastSaved")]
        public DateTime LastSaved;
    }
}