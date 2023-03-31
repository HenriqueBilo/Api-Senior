using Newtonsoft.Json;

namespace Api_Senior.Models
{
    public class OpenWeatherResponse
    {
        public MainData? Main { get; set; }

        public class MainData
        {
            [JsonProperty("temp")]
            public double Temperature { get; set; }
        }
    }
}
