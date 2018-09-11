using Newtonsoft.Json;
using System;

namespace pg.im.model.data
{
    public enum EMinerState
    {
        Idle = 0,
        Mining = 5,
        WalkingToMine = 10,
        WalkingToBin = 20
    }

    [Serializable]
    public class MinerRemoteData
    {
        [JsonProperty("MinedCash")]
        public double MinedCash;

        [JsonProperty("CurrentLocation")]
        public int CurrentLocation;

        [JsonProperty("MinerState")]
        public EMinerState MinerState;
    }
}