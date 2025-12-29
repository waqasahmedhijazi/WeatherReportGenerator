
using System.Text;
using WeatherReportGenerator.Application.DTOs;
using WeatherReportGenerator.Application.Interfaces;
using WebSupergoo.ABCpdf11;

namespace WeatherReportGenerator.Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        public PdfService()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("WeatherReportGenerator/1.0");
        }
        public byte[] GenerateWeatherReport(List<WeatherDto> weatherList, string city, string country)
        {
            if (weatherList == null)
                throw new ArgumentNullException(nameof(weatherList), "Weather list cannot be null");
            
                XSettings.InstallLicense("XeJREBodo/9R4nMEaKqFaomkebQdPdtypsbm8PToBk0g8HyNFHLtYOE4ekNzwUIFBw==");

                var doc = new Doc();

                var sb = new StringBuilder();
                sb.AppendLine("<!DOCTYPE html>");
                sb.AppendLine("<html>");
                sb.AppendLine("<head>");
                sb.AppendLine("<meta charset='UTF-8'>");
                sb.AppendLine("<style>");
                sb.AppendLine("body { font-family: Arial, sans-serif; margin: 10px; }");
                sb.AppendLine("h1 { color: #2c3e50; text-align: center; margin-bottom: 30px; }");
                sb.AppendLine("table { border: 0px; width: 100%; margin-top: 5px; }");
                sb.AppendLine("th, td {  padding: 5px; text-align: center; }");
                sb.AppendLine("hr {border: 1px solid #d3d3d3;}");
                sb.AppendLine("</style>");
                sb.AppendLine("</head>");
                sb.AppendLine("<body>");
                sb.AppendLine($"<h5>10-DAY WEATHER FORECAST FOR {city.ToUpper()}, {country.ToUpper()}</h5>");
                sb.AppendLine("<table>");

                foreach (var w in weatherList)
                {
                    string iconUrl = NormalizeIconUrl(w.IconUrl);
                    string subCondition = GetSubCondition(w);
                    string subConditionIcon = GetWeatherIcon(w.ConditionCode);

                    sb.AppendLine("<tr>");
                    sb.AppendLine($"<td><h5>{w.Date:dddd}</h5><span>{w.Date:dd/yyyy}</span></td>");
                    sb.AppendLine($"<td><img src=\"{iconUrl}\" alt=\"{w.Condition}\" width=\"64\" height=\"64\" /></td>");
                    sb.AppendLine($"<td><span style='font-size: 16;font-weight:bold'>{w.MaxTemperatureC}°</span> / {w.MinTemperatureC}°</td>");
                    sb.AppendLine($"<td><div><strong>{w.Condition}</strong></div><div style=\"font-size: 12px; color: #666; margin-top: 5px;\">{subConditionIcon} {subCondition}</div></td>");
                    sb.AppendLine($"<td>ω {w.Humidity}%</td>");
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<tr><td colspan=\"5\"><hr></td></tr>");
                }
                sb.AppendLine("</table>");
                sb.AppendLine("</body></html>");

                string html = sb.ToString();
                string base64Html = Convert.ToBase64String(Encoding.UTF8.GetBytes(html));
                string dataUri = $"data:text/html;base64,{base64Html}";
                int id = doc.AddImageUrl(dataUri);
                doc.MediaBox.String = "A4";
                while (doc.Chainable(id))
                {
                    doc.Page = doc.AddPage();
                    doc.MediaBox.String = "A4";
                    id = doc.AddImageToChain(id);
                }
                for (int i = 1; i <= doc.PageCount; i++)
                {
                    doc.PageNumber = i;
                    doc.Flatten();
                }

                byte[] pdf = doc.GetData();
                doc.Clear();

                return pdf;
        }
        private string GetWeatherIcon(int conditionCode)
        {
            if (conditionCode == 1000) return "☀️";
            if (conditionCode == 1003) return "⛅";
            if (conditionCode >= 1006 && conditionCode <= 1009) return "☁️";
            if (conditionCode >= 1063 && conditionCode <= 1195) return "🌧️";
            if (conditionCode >= 1210 && conditionCode <= 1225) return "🌨️";
            if (conditionCode >= 1240 && conditionCode <= 1264) return "🌦️";
            return "⛅";
        }
        private string NormalizeIconUrl(string iconUrl)
        {
            if (string.IsNullOrEmpty(iconUrl))
                return iconUrl;
            if (iconUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                iconUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return iconUrl;
            }
            if (iconUrl.StartsWith("//"))
            {
                return "https:" + iconUrl;
            }
            if (iconUrl.Contains("cdn.weatherapi.com"))
            {
                if (!iconUrl.StartsWith("http"))
                {
                    return "https:" + iconUrl;
                }
            }

            return iconUrl;
        }
        private string GetSubCondition(WeatherDto weather)
        {
            switch (weather.ConditionCode)
            {
                case 1000: return "Clear skies";
                case 1003: return "Partly cloudy";
                case 1006: return "Cloudy";
                case 1009: return "Overcast";
                case 1030: return "Mist";
                case 1063: return "Patchy rain";
                case 1066: return "Patchy snow";
                case 1069: return "Patchy sleet";
                case 1072: return "Patchy freezing drizzle";
                case 1087: return "Thundery outbreaks";
                case 1114: return "Blowing snow";
                case 1117: return "Blizzard";
                case 1135: return "Fog";
                case 1147: return "Freezing fog";
                case 1150: return "Patchy light drizzle";
                case 1153: return "Light drizzle";
                case 1168: return "Freezing drizzle";
                case 1171: return "Heavy freezing drizzle";
                case 1180: return "Patchy light rain";
                case 1183: return "Light rain";
                case 1186: return "Moderate rain";
                case 1189: return "Heavy rain";
                case 1192: return "Patchy heavy rain";
                case 1195: return "Heavy rain";
                case 1198: return "Light freezing rain";
                case 1201: return "Moderate/heavy freezing rain";
                case 1204: return "Light sleet";
                case 1207: return "Moderate/heavy sleet";
                case 1210: return "Patchy light snow";
                case 1213: return "Light snow";
                case 1216: return "Patchy moderate snow";
                case 1219: return "Moderate snow";
                case 1222: return "Patchy heavy snow";
                case 1225: return "Heavy snow";
                case 1237: return "Ice pellets";
                case 1240: return "Light rain shower";
                case 1243: return "Moderate/heavy rain shower";
                case 1246: return "Torrential rain shower";
                case 1249: return "Light sleet showers";
                case 1252: return "Moderate/heavy sleet showers";
                case 1255: return "Light snow showers";
                case 1258: return "Moderate/heavy snow showers";
                case 1261: return "Light showers of ice pellets";
                case 1264: return "Moderate/heavy showers of ice pellets";
                case 1273: return "Patchy light rain with thunder";
                case 1276: return "Moderate/heavy rain with thunder";
                case 1279: return "Patchy light snow with thunder";
                case 1282: return "Moderate/heavy snow with thunder";
                default: return $"Wind: {weather.WindSpeedKph:F0} kph";
            }
        }
    }
}