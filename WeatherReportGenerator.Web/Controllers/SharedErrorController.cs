using Microsoft.AspNetCore.Mvc;
using WeatherReportGenerator.Web.ViewModels;

namespace WeatherReportGenerator.Web.Controllers
{
    public class SharedErrorController : Controller
    {
        [Route("SharedError/BadRequest")]
        public IActionResult BadRequest(string message)
        {
            var model = new ErrorViewModel
            {
                ErrorMessage = message,
                RequestId = HttpContext.TraceIdentifier
            };
            return View("~/Views/Shared/BadRequest.cshtml", model);
        }

        [Route("SharedError/NotFound")]
        public IActionResult NotFound(string message)
        {
            var model = new ErrorViewModel
            {
                ErrorMessage = message,
                RequestId = HttpContext.TraceIdentifier
            };
            return View("~/Views/Shared/NotFound.cshtml", model);
        }

        [Route("SharedError/Error")]
        public IActionResult Error(string message)
        {
            var model = new ErrorViewModel
            {
                ErrorMessage = message,
                RequestId = HttpContext.TraceIdentifier
            };
            return View("~/Views/Shared/Error.cshtml", model);
        }
    }
}
