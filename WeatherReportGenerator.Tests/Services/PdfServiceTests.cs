using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherReportGenerator.Application.DTOs;
using WeatherReportGenerator.Infrastructure.Services;

namespace WeatherReportGenerator.Tests.Services
{
    public class PdfServiceTests
    {
        private readonly PdfService _pdfService;

        public PdfServiceTests()
        {
            _pdfService = new PdfService();
        }

        [Fact]
        public void GenerateWeatherReport_ShouldReturnByteArray_WhenValidWeatherListProvided()
        {
            // Arrange
            var weatherList = new List<WeatherDto>
            {
                new WeatherDto
                {
                    Date = DateTime.Today,
                    MaxTemperatureC = 25,
                    MinTemperatureC = 15,
                    Humidity = 60,
                    WindSpeedKph = 10,
                    Condition = "Sunny",
                    IconUrl = "https://cdn.weatherapi.com/weather/sunny.png",
                    ConditionCode = 1000
                },
                new WeatherDto
                {
                    Date = DateTime.Today.AddDays(1),
                    MaxTemperatureC = 27,
                    MinTemperatureC = 16,
                    Humidity = 65,
                    WindSpeedKph = 12,
                    Condition = "Cloudy",
                    IconUrl = "https://cdn.weatherapi.com/weather/cloudy.png",
                    ConditionCode = 1006
                }
            };

            string city = "Lahore";
            string country = "Pakistan";

            // Act
            var result = _pdfService.GenerateWeatherReport(weatherList, city, country);

            // Assert
            result.Should().NotBeNull();
            result.Length.Should().BeGreaterThan(0); // PDF bytes should exist
        }

        [Fact]
        public void GenerateWeatherReport_ShouldThrowException_WhenWeatherListIsNull()
        {
            // Arrange
            List<WeatherDto> weatherList = null;
            string city = "Lahore";
            string country = "Pakistan";

            // Act
            Action act = () => _pdfService.GenerateWeatherReport(weatherList, city, country);

            // Assert
            act.Should().Throw<ArgumentNullException>()
               .WithMessage("Weather list cannot be null*");
        }
    }
}
