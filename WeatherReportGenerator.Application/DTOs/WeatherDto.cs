

namespace WeatherReportGenerator.Application.DTOs
{
    using System;
    using System.Text.Json.Serialization;

    public class WeatherDto
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("maxtemp_c")]
        public double MaxTemperatureC { get; set; }

        [JsonPropertyName("mintemp_c")]
        public double MinTemperatureC { get; set; }

        [JsonPropertyName("wind_kph")]
        public double WindSpeedKph { get; set; }

        [JsonPropertyName("icon")]
        public string IconUrl { get; set; }

        [JsonPropertyName("code")]
        public int ConditionCode { get; set; }

        [JsonPropertyName("precip_mm")] 
        public double Precipitation { get; set; }
        
        [JsonPropertyName("condition")]
        public string Condition { get; set; }

        [JsonPropertyName("sub_condition")]
        public string SubCondition { get; set; }

        [JsonPropertyName("humidity")]
        public double Humidity { get; set; }
    }
}
