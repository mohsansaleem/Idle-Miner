using Newtonsoft.Json;

namespace pg.im.model.data
{
    public class WarehouseLevelData
    {
        [JsonProperty("Transporters")]
        public float Transporters;

        [JsonProperty("LoadingSpeed")]
        public double LoadingSpeed;

        [JsonProperty("LoadperTransporter")]
        public double LoadPerTransporter;

        [JsonProperty("Walkspeed")]
        public float WalkSpeed;

        [JsonProperty("UpgradeCost")]
        public double UpgradeCost;
    }
}