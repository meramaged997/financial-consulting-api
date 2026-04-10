using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using startawy.Core.Enums;
using Startawy.Application.Common;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.Interfaces;

namespace Startawy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "PremiumOnly")]
public class MarketingController : BaseController
{
    private readonly IMarketingService _service;

    public MarketingController(IMarketingService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMarketingCampaignRequest request, CancellationToken ct = default)
    {
        var result = await _service.CreateAsync(CurrentUserId, request, ct);
        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created,
                ApiResponse<object>.Ok(result.Value!, "Marketing campaign created."))
            : BadRequest(ApiResponse<object>.Fail(result.Error ?? "Request failed."));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
        => HandleResult(await _service.GetAllAsync(CurrentUserId, ct));

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] CampaignStatus status, CancellationToken ct = default)
        => HandleResult(await _service.UpdateStatusAsync(CurrentUserId, id, status, ct));
}
