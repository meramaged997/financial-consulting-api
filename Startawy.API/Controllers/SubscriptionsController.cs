using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.Interfaces;

namespace Startawy.API.Controllers;

[ApiController]
[Route("api/subscriptions")]
[Authorize]
public class SubscriptionsController : BaseController
{
    private readonly ISubscriptionService _service;

    public SubscriptionsController(ISubscriptionService service) => _service = service;

    [HttpGet("me")]
    public async Task<IActionResult> GetMine(CancellationToken ct = default)
        => HandleResult(await _service.GetMySubscriptionAsync(CurrentUserId, ct));

    [HttpPost("upgrade")]
    public async Task<IActionResult> Upgrade([FromBody] UpgradePackageRequest request, CancellationToken ct = default)
        => HandleResult(await _service.UpgradeAsync(CurrentUserId, request, ct));
}

