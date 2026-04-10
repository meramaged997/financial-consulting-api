using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Startawy.API.Controllers;

[ApiController]
[Route("api/debug")]
public class DebugController : ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        return Ok(new { success = true, message = "Authenticated", data = new { claims } });
    }
}
