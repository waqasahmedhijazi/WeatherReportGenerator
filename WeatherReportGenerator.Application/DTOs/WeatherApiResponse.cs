

namespace WeatherReportGenerator.Application.DTOs
{
    public class WeatherApiResponse
    {
        public double Temperature { get; set; }
        public string Condition { get; set; }
        public double Humidity { get; set; }
    }
}
