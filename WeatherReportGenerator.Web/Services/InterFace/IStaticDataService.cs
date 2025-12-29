using WeatherReportGenerator.Web.ViewModels;

namespace WeatherReportGenerator.Web.Services.InterFace
{
    public interface IStaticDataService
    {
        List<CountryViewModel> GetCountries();
    }
}
