using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.Interfaces;

namespace Startawy.API.Controllers;

[ApiController]
[Route("api/follow-up-plans")]
[Authorize]
public class FollowUpPlansController : BaseController
{
    private readonly IFollowUpPlanService _service;

    public FollowUpPlansController(IFollowUpPlanService service) => _service = service;

    // Consultants create structured follow-up plans for Premium founders.
    [HttpPost]
    [Authorize(Roles = "FinancialConsultant")]
    public async Task<IActionResult> Create([FromBody] CreateFollowUpPlanRequest request, CancellationToken ct = default)
        => HandleResult(await _service.CreateAsync(CurrentUserId, request, ct));

    // Founders view their plans.
    [HttpGet("mine")]
    [Authorize(Roles = "StartupFounder")]
    public async Task<IActionResult> GetMine(CancellationToken ct = default)
        => HandleResult(await _service.GetForFounderAsync(CurrentUserId, ct));
}

