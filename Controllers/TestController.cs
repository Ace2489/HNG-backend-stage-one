using Microsoft.AspNetCore.Mvc;

namespace HNG_backend_stage_one.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public IActionResult Hello([FromQuery] string visitor)
    {
        string queryString = visitor;

        return Ok(new { Message = string.Format("Hello, {0}", visitor)});
    }
}
