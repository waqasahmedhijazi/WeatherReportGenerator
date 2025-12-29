
using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherReportGenerator.Application.DTOs;
using WeatherReportGenerator.Application.Interfaces;
using WeatherReportGenerator.Application.Settings;


namespace WeatherReportGenerator.Infrastructure.Clients
{
    public class WeatherApiClient : IWeatherApiClient
    {
        private readonly HttpClient _http;
        private readonly WeatherApiSettings _settings;

        public WeatherApiClient(HttpClient http, IOptions<WeatherApiSettings> options)
        {
            _http = http;
            _settings = options.Value;
        }

        public async Task<List<WeatherDto>> GetForecastAsync(string city, string country, int days)
        {
            string query = $"{city},{country}";
            string url = $"{_settings.BaseUrl}/forecast.json?key={_settings.ApiKey}&q={query}&days={days}";
            using var responseStream = await _http.GetStreamAsync(url);
            using var doc = await JsonDocument.ParseAsync(responseStream);
            var result = new List<WeatherDto>();
            if (doc.RootElement.TryGetProperty("forecast", out var forecastElement) &&
                forecastElement.TryGetProperty("forecastday", out var forecastDays) &&
                forecastDays.ValueKind == JsonValueKind.Array)
            {
                foreach (var dayElement in forecastDays.EnumerateArray())
                {
                    try
                    {
                        var day = dayElement.GetProperty("day");
                        var condition = day.GetProperty("condition");

                        var weather = new WeatherDto
                        {
                            Date = dayElement.TryGetProperty("date", out var dateProp) && DateTime.TryParse(dateProp.GetString(), out var date)
                                ? date
                                : DateTime.MinValue,

                            MaxTemperatureC = day.TryGetProperty("maxtemp_c", out var maxTempProp) ? maxTempProp.GetDouble() : 0,
                            MinTemperatureC = day.TryGetProperty("mintemp_c", out var minTempProp) ? minTempProp.GetDouble() : 0,
                            Humidity = day.TryGetProperty("avghumidity", out var humidityProp) ? humidityProp.GetDouble() : 0,
                            WindSpeedKph = day.TryGetProperty("maxwind_kph", out var windProp) ? windProp.GetDouble() : 0,

                            Condition = condition.TryGetProperty("text", out var textProp) ? textProp.GetString() ?? string.Empty : string.Empty,
                            IconUrl = condition.TryGetProperty("icon", out var iconProp) ? "https:" + (iconProp.GetString() ?? "") : string.Empty,
                            ConditionCode = condition.TryGetProperty("code", out var codeProp) ? codeProp.GetInt32() : 0
                        };

                        result.Add(weather);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return result;
        }
    }

}
