using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dyvenix.App1.Core.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { 
            service = "App1",
            status = "healthy",
            timestamp = DateTime.UtcNow 
        });
    }

    [HttpGet("secure")]
    [Authorize]
    public IActionResult GetSecure()
    {
        return Ok(new { 
            service = "App1",
            status = "healthy (authenticated)",
            user = User.Identity?.Name,
            timestamp = DateTime.UtcNow 
        });
    }
}
