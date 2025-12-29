using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using WeatherReportGenerator.Application.Interfaces;
using WeatherReportGenerator.Web.Services.InterFace;

namespace WeatherReportGenerator.Web.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherService _weatherService;
        private readonly IPdfService _pdfService;
        private readonly IStaticDataService _staticDataService;

        public WeatherController(
            IWeatherService weatherService,
            IPdfService pdfService,
            IStaticDataService staticDataService)
        {
            _weatherService = weatherService;
            _pdfService = pdfService;
            _staticDataService = staticDataService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var getCountries = _staticDataService.GetCountries();
            return View(getCountries);
        }

        [HttpGet]
        public async Task<IActionResult> GetWeatherData(string city, string country)
        {
            var weather = await _weatherService.GetWeatherAsync(city, country);
            return Ok(weather);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadPdf(string city, string country)
        {
            var weather = await _weatherService.GetWeatherAsync(city, country);
            var pdfBytes = _pdfService.GenerateWeatherReport(weather, city, country);
            return File(pdfBytes, "application/pdf", "Weather-Forecast.pdf");
        }
        [HttpGet]   
        public IActionResult Error()
        {
            ViewBag.GlobalError = HttpContext.Items["GlobalError"] ?? "An unexpected error occurred.";
            return View();
        }
    }
}
