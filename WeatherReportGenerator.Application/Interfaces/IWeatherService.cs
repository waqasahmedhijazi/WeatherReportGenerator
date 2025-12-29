
using WeatherReportGenerator.Application.DTOs;

namespace WeatherReportGenerator.Application.Interfaces
{
    public interface IWeatherService
    {
        Task<List<WeatherDto>> GetWeatherAsync(string city, string country);
    }
}
