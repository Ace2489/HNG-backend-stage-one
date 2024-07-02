using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace HNG_backend_stage_one.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public IActionResult Hello([FromQuery] string visitor)
    {

        string ipAddress = GetIPAddress();
        return Ok(new
        {
            client_ip = ipAddress,
            location = "Not yet implemented",
            greeting = $"Hello, {visitor}"
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
}
