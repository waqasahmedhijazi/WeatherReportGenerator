using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherReportGenerator.Application.DTOs;
using WeatherReportGenerator.Application.Interfaces;
using WeatherReportGenerator.Web.Controllers;
using WeatherReportGenerator.Web.Services.InterFace;
using WeatherReportGenerator.Web.ViewModels;

namespace WeatherReportGenerator.Tests.Controller
{
    public class WeatherControllerTests
    {
        private readonly Mock<IWeatherService> _weatherServiceMock;
        private readonly Mock<IPdfService> _pdfServiceMock;
        private readonly Mock<IStaticDataService> _staticDataServiceMock;
        private readonly WeatherController _controller;

        public WeatherControllerTests()
        {
            _weatherServiceMock = new Mock<IWeatherService>();
            _pdfServiceMock = new Mock<IPdfService>();
            _staticDataServiceMock = new Mock<IStaticDataService>();

            _controller = new WeatherController(
                _weatherServiceMock.Object,
                _pdfServiceMock.Object,
                _staticDataServiceMock.Object
            );
        }

        [Fact]
        public void Index_ShouldReturnViewWithCountries()
        {
            var countries = new List<CountryViewModel>
        {
            new CountryViewModel { Code = "PK", Name = "Pakistan" },
            new CountryViewModel { Code = "USA", Name = "USA" }
        };
            _staticDataServiceMock.Setup(s => s.GetCountries()).Returns(countries);

            var result = _controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            viewResult.Model.Should().BeEquivalentTo(countries);
        }

        [Fact]
        public async Task GetWeatherData_ShouldReturnOk_WhenWeatherDataExists()
        {
            var weatherList = new List<WeatherDto>
    {
        new WeatherDto { Date = DateTime.Today, MaxTemperatureC = 30, MinTemperatureC = 20 }
    };
            _weatherServiceMock.Setup(s => s.GetWeatherAsync("Lahore", "Pakistan"))
                               .ReturnsAsync(weatherList);

            var result = await _controller.GetWeatherData("Lahore", "Pakistan");

            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(weatherList);
        }


        [Fact]
        public async Task DownloadPdf_ShouldReturnFile_WhenWeatherDataExists()
        {
            var weatherList = new List<WeatherDto> { new WeatherDto { Date = DateTime.Today } };
            _weatherServiceMock.Setup(s => s.GetWeatherAsync("Lahore", "Pakistan"))
                               .ReturnsAsync(weatherList);

            _pdfServiceMock.Setup(p => p.GenerateWeatherReport(weatherList, "Lahore", "Pakistan"))
                           .Returns(new byte[] { 1, 2, 3 });

            var result = await _controller.DownloadPdf("Lahore", "Pakistan");

            var fileResult = Assert.IsType<FileContentResult>(result);
            fileResult.ContentType.Should().Be("application/pdf");
            fileResult.FileDownloadName.Should().Be("Weather-Forecast.pdf");
            fileResult.FileContents.Should().BeEquivalentTo(new byte[] { 1, 2, 3 });
        }

        [Fact]
        public void Error_ShouldReturnView_WithGlobalError()
        {
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.HttpContext.Items["GlobalError"] = "Something went wrong";

            var result = _controller.Error();

            var viewResult = Assert.IsType<ViewResult>(result);
            ((string)_controller.ViewBag.GlobalError).Should().Be("Something went wrong");
        }
    }
}
