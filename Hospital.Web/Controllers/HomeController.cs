using Hospital.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Hospital.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Read the base URL from appsettings.json
                string baseUrl = _configuration["BaseUrl"];

                // Ensure the base URL is not null or empty
                if (string.IsNullOrEmpty(baseUrl))
                {
                    return BadRequest("Base URL not found in appsettings.json.");
                }

                // The endpoint you want to make the POST request to
                string endpoint = "api/Appointment/Available?date=2023-07-28";

                // Create the full URL by combining the base URL and endpoint
                string url = $"{baseUrl.TrimEnd('/')}/{endpoint}";

                // Create an instance of HttpClient (consider using a singleton pattern in real applications)
                using (var httpClient = new HttpClient())
                {
                    // Optionally, you can set headers for the request
                    httpClient.DefaultRequestHeaders.Add("accept", "text/plain");

                    // If there is a request payload (data to be sent), you can set it using StringContent
                    // For this example, the payload is empty.
                    string payload = string.Empty;
                    var content = new StringContent(payload);

                    // Send the POST request and get the response
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);

                    // Check if the response was successful (status code in the 2xx range)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var responseToView= JsonConvert.DeserializeObject<Hospital.Shared.Shared.ServiceActionResult<List<Get_AvailableDoctors_Response>>>(responseBody);
                        return View(responseBody);
                    }
                    else
                    {
                        return BadRequest($"Request failed with status code: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Request error: {ex.Message}");
            }
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}