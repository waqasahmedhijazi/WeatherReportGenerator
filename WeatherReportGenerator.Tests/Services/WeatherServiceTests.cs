using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherReportGenerator.Application.DTOs;
using WeatherReportGenerator.Application.Interfaces;
using WeatherReportGenerator.Infrastructure.Clients;
using WeatherReportGenerator.Infrastructure.Services;

namespace WeatherReportGenerator.Tests.Services
{
    public class WeatherServiceTests
    {
        private readonly Mock<IWeatherApiClient> _weatherApiClientMock;
        private readonly WeatherService _service;

        public WeatherServiceTests()
        {
            _weatherApiClientMock = new Mock<IWeatherApiClient>();
            _service = new WeatherService(_weatherApiClientMock.Object);
        }

        [Fact]
        public async Task GetWeatherAsync_ShouldThrowArgumentException_WhenCityIsEmpty()
        {
            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.GetWeatherAsync("", "Pakistan"));
            Assert.Equal("City is required. (Parameter 'city')", ex.Message);
        }

        [Fact]
        public async Task GetWeatherAsync_ShouldThrowArgumentException_WhenCountryIsEmpty()
        {
            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(
                () => _service.GetWeatherAsync("Islamabad", ""));
            Assert.Equal("Country is required. (Parameter 'country')", ex.Message);
        }

        [Fact]
        public async Task GetWeatherAsync_ShouldThrowCityNotFoundException_WhenApiReturnsEmptyList()
        {
            // Arrange
            _weatherApiClientMock
                .Setup(c => c.GetForecastAsync("InvalidCity", "Pakistan", 10))
                .ReturnsAsync(new List<WeatherDto>());

            // Act & Assert
            await Assert.ThrowsAsync<CityNotFoundException>(
                () => _service.GetWeatherAsync("InvalidCity", "Pakistan"));
        }

        [Fact]
        public async Task GetWeatherAsync_ShouldThrowWeatherServiceException_WhenHttpRequestFails()
        {
            // Arrange
            _weatherApiClientMock
                .Setup(c => c.GetForecastAsync("Islamabad", "Pakistan", 10))
                .ThrowsAsync(new System.Net.Http.HttpRequestException("API down"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<WeatherServiceException>(
                () => _service.GetWeatherAsync("Islamabad", "Pakistan"));
            Assert.Equal("Unable to fetch weather data. Try again later.", ex.Message);
        }

        [Fact]
        public async Task GetWeatherAsync_ShouldReturnWeatherList_WhenApiReturnsData()
        {
            // Arrange
            var weatherList = new List<WeatherDto>
            {
                new WeatherDto { Date = DateTime.Today, MaxTemperatureC = 30, MinTemperatureC = 20 }
            };

            _weatherApiClientMock
                .Setup(c => c.GetForecastAsync("Islamabad", "Pakistan", 10))
                .ReturnsAsync(weatherList);

            // Act
            var result = await _service.GetWeatherAsync("Islamabad", "Pakistan");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(30, result[0].MaxTemperatureC);
            Assert.Equal(20, result[0].MinTemperatureC);
        }
    }
}
