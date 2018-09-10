using Newtonsoft.Json;

namespace pg.im.model.data
{
    public class ShaftLevelData
    {
        [JsonProperty("Miners")]
        public int Miners;

        [JsonProperty("WalkSpeed")]
        public float WalkSpeed;

        [JsonProperty("MinningSpeed")]
        public double MinningSpeed;

        [JsonProperty("WorkerCapacity")]
        public double WorkerCapacity;

        [JsonProperty("UpgradeCost")]
        public double UpgradeCost;
    }
}