
using WeatherReportGenerator.Application.DTOs;

namespace WeatherReportGenerator.Application.Interfaces
{
    public interface IWeatherApiClient
    {
        Task<List<WeatherDto>> GetForecastAsync(string city, string country, int days);
    }
}
