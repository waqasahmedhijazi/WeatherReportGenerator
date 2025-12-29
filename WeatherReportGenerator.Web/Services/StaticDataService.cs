using Newtonsoft.Json;
using WeatherReportGenerator.Web.Services.InterFace;
using WeatherReportGenerator.Web.ViewModels;

namespace WeatherReportGenerator.Web.Services
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IWebHostEnvironment _envService;

        public StaticDataService(IWebHostEnvironment envService)
        {
            _envService = envService;
        }

        public List<CountryViewModel> GetCountries()
        {
            var filePath = Path.Combine(_envService.WebRootPath, "static-data", "countries.json");
            if (!File.Exists(filePath))
                return new List<CountryViewModel>();

            var jsonData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<CountryViewModel>>(jsonData)
                   ?? new List<CountryViewModel>();
        }
    }
}
