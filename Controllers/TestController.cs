using System.Net;
using System.Text.Json;
using HNG_backend_stage_one.Models;
using Microsoft.AspNetCore.Mvc;

namespace HNG_backend_stage_one.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloController(IHttpClientFactory httpClientFactory, IConfiguration configuration) : ControllerBase
{
    private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
    private readonly IConfiguration configuration = configuration;

    [HttpGet]
    public async Task<IActionResult> Hello([FromQuery] string visitor)
    {

        string ipAddress = GetIPAddress();

        LocationInfo locationInfo = await GetLocationAsync(ipAddress);

        if (!string.IsNullOrEmpty(locationInfo.Error) || string.IsNullOrEmpty(locationInfo.City))
        {
            return BadRequest(new { message = "Unable to determine location data for your IP address" });
        }

        Temperature temperature = await GetTemperatureAsync(locationInfo);
        if (!string.IsNullOrEmpty(temperature.Error))
        {
            return BadRequest(new { Message = "Unable to determine temperature data for your location" });
        }

        string location = locationInfo.City;
        return Ok(new
        {
            client_ip = ipAddress,
            location,
            greeting = $"Hello, {visitor}!, the temperature is {temperature.Temp} degrees celsius in {location}"
        });
    }

    private string GetIPAddress()
    {
        IPAddress? iPAddress = HttpContext.Connection.RemoteIpAddress;
        if (iPAddress == null)
        {
            return "unknown";
        }
        if (Request.Headers.TryGetValue("X-Forwarded-For", out Microsoft.Extensions.Primitives.StringValues value))
        {
            return value!;
        }
        return iPAddress.ToString();
    }

    private async Task<LocationInfo> GetLocationAsync(string ipAddress)
    {

        Console.WriteLine("\nIP DATA:{0}\n", ipAddress);
        string locationAPI = new($"http://ip-api.com/json/{ipAddress}");
        HttpClient client = httpClientFactory.CreateClient("locationClient");
        HttpResponseMessage response = await client.GetAsync(locationAPI);
        LocationInfo? info = null;

        if (response.IsSuccessStatusCode)
        {
            using Stream content = await response.Content.ReadAsStreamAsync();
            info = await JsonSerializer.DeserializeAsync<LocationInfo>(content);
        }
        return info ?? new LocationInfo() { Error = "Error" };
    }

    private async Task<Temperature> GetTemperatureAsync(LocationInfo location)
    {   
        Console.WriteLine("\nLOCATION DATA:{0}, {1}\n", location.Longitude, location.Latitude);
        string apiKey = configuration.GetValue<string>("WEATHER_API_KEY")!;
        string weatherAPI = new($"https://api.openweathermap.org/data/2.5/weather?lat={location.Latitude}&lon={location.Longitude}&appid={apiKey}&units=metric");
        HttpClient client = httpClientFactory.CreateClient("weatherClient");
        HttpResponseMessage response = await client.GetAsync(weatherAPI);
        Console.WriteLine("RESPONSE CODE:{0}\nRESPONSE BODY: {1}", response.StatusCode, response.Content.ReadAsStringAsync());
        Temperature? temperature = null;
        Temperature errorTemp = new() { Error = "Error" };
        if (response.IsSuccessStatusCode)
        {
            using Stream content = await response.Content.ReadAsStreamAsync();
            Root root = await JsonSerializer.DeserializeAsync<Root>(content) ?? new Root();
            temperature = root.Temperature;
        }
        return temperature ?? errorTemp;
    }
}
