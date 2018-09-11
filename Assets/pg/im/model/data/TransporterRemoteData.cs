using Newtonsoft.Json;
using System;

namespace pg.im.model.data
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
        public int CurrentLocation;

        [JsonProperty("TransporterState")]
        public ETransporterState TransporterState;
    }
}