using Newtonsoft.Json;

namespace PG.IdleMiner.Models.DataModels
{
    public class ElevatorLevelData
    {
        [JsonProperty("MovementSpeed")]
        public int MovementSpeed;

        [JsonProperty("LoadingSpeed")]
        public double LoadingSpeed;

        [JsonProperty("Load")]
        public double LoadCapacity;

        [JsonProperty("UpgradeCost")]
        public double UpgradeCost;
    }
}