using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Startawy.Application.Interfaces;

namespace Startawy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : BaseController
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct = default)
        => HandleResult(await _service.GetAsync(CurrentUserId, ct));
}
