using System;
using System.Text.Json.Serialization;

namespace Frontend.Models
{
    public class WeatherForecast
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("temperatureC")]
        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        [JsonPropertyName("summary")]
        public string Summary { get; set; }
    }
}
