using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WeatherReportGenerator.Infrastructure.Services;
using WeatherReportGenerator.Web.ViewModels; 

namespace WeatherReportGenerator.Web.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                LogToFile(ex);

                string viewUrl;
                int statusCode;
                string friendlyMessage;

                switch (ex)
                {
                    case ArgumentException:
                        statusCode = 400;
                        viewUrl = "/SharedError/BadRequest";
                        friendlyMessage = ex.Message;
                        break;

                    case CityNotFoundException:
                        statusCode = 404;
                        viewUrl = "/SharedError/NotFound";
                        friendlyMessage = ex.Message;
                        break;

                    case WeatherServiceException:
                        statusCode = 503;
                        viewUrl = "/SharedError/Error";
                        friendlyMessage = ex.Message;
                        break;

                    default:
                        statusCode = 500;
                        viewUrl = "/SharedError/Error";
                        friendlyMessage = "Something went wrong. Please try again.";
                        break;
                }

                var model = new ErrorViewModel
                {
                    RequestId = context.TraceIdentifier,
                    ErrorMessage = friendlyMessage,
                    RedirectUrl = "/"
                };

                context.Items["GlobalError"] = friendlyMessage;
                context.Response.Clear();
                context.Response.StatusCode = statusCode;

                bool isAjax = context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

                if (isAjax)
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new
                    {
                        redirectUrl = viewUrl,
                        message = friendlyMessage
                    });
                }
                else
                {
                    var actionContext = new ActionContext(
                        context,
                        context.GetRouteData() ?? new RouteData(),
                        new ControllerActionDescriptor()
                    );

                    var viewResult = new ViewResult
                    {
                        ViewName = viewUrl,
                        ViewData = new ViewDataDictionary<ErrorViewModel>(
                            new EmptyModelMetadataProvider(),
                            new ModelStateDictionary())
                        {
                            Model = model
                        }
                    };

                    await viewResult.ExecuteResultAsync(actionContext);
                }
            }
        }
        private void LogToFile(Exception exception)
        {
            var logDir = Path.Combine(_env.ContentRootPath, "Logs");

            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);

            var logFile = Path.Combine(logDir, $"log-{DateTime.Now:yyyy-MM-dd}.txt");

            var logMessage = $@"==========================
                            Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}
                            Type: {exception.GetType().Name}
                            Message: {exception.Message}
                            StackTrace:
                            {exception.StackTrace}
                            ==========================";

            File.AppendAllText(logFile, logMessage);
        }
    }
}
