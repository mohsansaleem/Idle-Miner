using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pg.im.model.data
{
    [Serializable]
    public class UserData
    {
        [JsonProperty("Shafts")]
        public List<ShaftRemoteData> UserShafts;

        [JsonProperty("ManagersUnlocked")]
        public ManagersRemoteData Managers;

        [JsonProperty("IdleCash")]
        public double IdleCash;

        [JsonProperty("Cash")]
        public double Cash;

        [JsonProperty("SuperCash")]
        public double SuperCash;
    }
}