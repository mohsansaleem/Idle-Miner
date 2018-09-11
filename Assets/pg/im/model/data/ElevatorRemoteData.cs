using Newtonsoft.Json;
using System;

namespace pg.im.model.data
{
    public enum EElevatorState
    {
        Idle = 0,
        Loading = 5,
        MovingDown = 10,
        MovingUp = 20
    }

    [Serializable]
    public class ElevatorRemoteData
    {
        [JsonProperty("ElevatorLevel")]
        public int ElevatorLevel;

        [JsonProperty("StoredCash")]
        public double StoredCash;

        [JsonProperty("LoadedCash")]
        public double LoadedCash;

        [JsonProperty("CurrentLocation")]
        public int CurrentLocation;

        [JsonProperty("Manager")]
        public string Manager;

        [JsonProperty("ElevatorState")]
        public EElevatorState ElevatorState;


        [JsonIgnore]
        public ElevatorLevelData ElevatorLevelData { get; set; }
    }
}