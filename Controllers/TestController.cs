using Microsoft.AspNetCore.Mvc;

namespace HNG_backend_stage_one.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController(ILogger<TestController> logger) : ControllerBase
{
    private readonly ILogger<TestController> _logger = logger;

    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Get()
    {
       return Ok();
    }
}
