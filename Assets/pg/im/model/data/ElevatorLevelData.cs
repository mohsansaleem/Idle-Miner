using Newtonsoft.Json;

namespace pg.im.model.data
{
    public class ElevatorLevelData
    {
        [JsonProperty("MovementSpeed")]
        public float MovementSpeed;

        [JsonProperty("LoadingSpeed")]
        public double LoadingSpeed;

        [JsonProperty("Load")]
        public double LoadCapacity;

        [JsonProperty("UpgradeCost")]
        public double UpgradeCost;
    }
}