
using WeatherReportGenerator.Application.DTOs;
using WeatherReportGenerator.Application.Interfaces;

namespace WeatherReportGenerator.Infrastructure.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherApiClient _client;

        public WeatherService(IWeatherApiClient client)
        {
            _client = client;
        }
        public async Task<List<WeatherDto>> GetWeatherAsync(string city, string country)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City is required.", nameof(city));

            if (string.IsNullOrWhiteSpace(country))
                throw new ArgumentException("Country is required.", nameof(country));

            try
            {
                var weather = await _client.GetForecastAsync(city, country, 10);

                if (weather == null || !weather.Any())
                    throw new CityNotFoundException(city);

                return weather;
            }
            catch (HttpRequestException ex)
            {
                throw new WeatherServiceException("Unable to fetch weather data. Try again later.", ex);
            }
        }

    }
    public class CityNotFoundException : Exception
    {
        public CityNotFoundException(string city)
            : base($"City '{city}' was not found.") { }
    }
    public class WeatherServiceException : Exception
    {
        public WeatherServiceException(string message, Exception inner = null)
            : base(message, inner) { }
    }
}
