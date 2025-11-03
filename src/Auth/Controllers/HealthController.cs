using Microsoft.AspNetCore.Mvc;

namespace Dyvenix.Auth.Core.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { 
            service = "Auth",
            status = "healthy",
            timestamp = DateTime.UtcNow 
        });
    }
}
