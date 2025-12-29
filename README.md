# Weather Report Generator - Waqas Hijazi



\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_



**Application Setup \& Usage Instructions Prerequisites**



.NET SDK installed (compatible with the solution)



Visual Studio 2022 (or later) / Rider / VS Code



Internet connection (required for weather API)

**Project Structure Overview**



The solution follows a clean, layered architecture:



Application – Business logic, DTOs, interfaces, and settings



Infrastructure – External services, API clients, and implementations



Web – ASP.NET Core MVC application (UI and middleware)



Unit Tests – Automated unit tests for application and web layers



**Steps to Run the Application**



Clone the Repository



Open the Solution



Navigate to the cloned folder



Open the .sln file in Visual Studio



Restore Dependencies



NuGet packages will restore automatically



If not, run:



dotnet restore





Run the Application



Set the Web project as the startup project



Press F5 or click Run



Launch in Browser



The application will automatically open in your default browser



**How to Use the Application**



On the home page, select a country from the dropdown



Enter the city name



Click the Search button to view the weather details



Once results are displayed, click Download PDF



A formatted weather report PDF will be generated and downloaded



**Notes**



Global exception handling and logging are enabled



Friendly error pages are displayed for handled errors



PDF generation uses the configured PDF library



Weather data is fetched from the configured external API

