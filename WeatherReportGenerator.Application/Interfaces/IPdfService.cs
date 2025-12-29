
using WeatherReportGenerator.Application.DTOs;

namespace WeatherReportGenerator.Application.Interfaces
{
    public interface IPdfService
    {
        byte[] GenerateWeatherReport(List<WeatherDto> weatherList, string city, string country);
    }
}
