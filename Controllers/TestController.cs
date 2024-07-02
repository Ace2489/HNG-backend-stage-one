using Microsoft.AspNetCore.Mvc;

namespace HNG_backend_stage_one.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloController : ControllerBase
{
    [HttpGet("hello")]
    public IActionResult Hello([FromQuery] string visitor)
    {
       return Ok();
    }
}
