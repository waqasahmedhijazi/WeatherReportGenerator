namespace WeatherReportGenerator.Web.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public string ErrorMessage { get; set; }
        public string RedirectUrl { get; set; } = "/"; 
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
