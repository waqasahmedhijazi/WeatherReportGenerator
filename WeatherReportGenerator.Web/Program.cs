
using System.Text.Json;
using WeatherReportGenerator.Application.Interfaces;
using WeatherReportGenerator.Application.Settings;
using WeatherReportGenerator.Infrastructure.Clients;
using WeatherReportGenerator.Infrastructure.Services;
using WeatherReportGenerator.Web.Middlewares;
using WeatherReportGenerator.Web.Services;
using WeatherReportGenerator.Web.Services.InterFace;

namespace WeatherReportGenerator.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<WeatherApiSettings>(
                builder.Configuration.GetSection("WeatherApi"));
            builder.Services.AddHttpClient<WeatherApiClient>();
            builder.Services.AddScoped<IWeatherService, WeatherService>();
            builder.Services.AddScoped<IPdfService, PdfService>();
            builder.Services.AddScoped<IStaticDataService, StaticDataService>();
            builder.Services.AddScoped<IWeatherApiClient, WeatherApiClient>();

            builder.Services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy =
                        JsonNamingPolicy.CamelCase;
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();
            app.UseMiddleware<GlobalExceptionMiddleware>();

            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Weather}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
