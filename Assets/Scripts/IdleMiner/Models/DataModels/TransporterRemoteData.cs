using System;
using Newtonsoft.Json;

namespace PG.IdleMiner.Models.DataModels
{
    public enum ETransporterState
    {
        Idle = 0,
        Loading = 5,
        WalkingToElevator = 10,
        WalkingToWarehouse = 20
    }

    [Serializable]
    public class TransporterRemoteData
    {
        [JsonProperty("LoadedCash")]
        public double LoadedCash;

        [JsonProperty("CurrentLocation")]
        public float CurrentLocation;

        [JsonProperty("TransporterState")]
        public ETransporterState TransporterState;
    }
}