using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.Interfaces;

namespace Startawy.API.Controllers;

[ApiController]
[Route("api/sessions")]
[Authorize]
public class SessionsController : BaseController
{
    private readonly ISessionsService _service;

    public SessionsController(ISessionsService service) => _service = service;

    // Consultant sets availability slots
    [HttpPost("availability")]
    [Authorize(Roles = "FinancialConsultant")]
    public async Task<IActionResult> CreateAvailability([FromBody] CreateAvailabilitySlotRequest request, CancellationToken ct = default)
        => HandleResult(await _service.CreateAvailabilityAsync(CurrentUserId, request, ct));

    // Anyone can view a consultant's open availability window
    [HttpGet("availability/{consultantUserId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAvailability(string consultantUserId, [FromQuery] DateTime fromUtc, [FromQuery] DateTime toUtc, CancellationToken ct = default)
        => HandleResult(await _service.GetAvailabilityAsync(consultantUserId, fromUtc, toUtc, ct));

    // Founder books a session (paid separately)
    [HttpPost("book")]
    [Authorize(Roles = "StartupFounder")]
    public async Task<IActionResult> Book([FromBody] BookSessionRequest request, CancellationToken ct = default)
        => HandleResult(await _service.BookAsync(CurrentUserId, request, ct));

    [HttpGet("mine")]
    [Authorize(Roles = "StartupFounder")]
    public async Task<IActionResult> GetMineFounder(CancellationToken ct = default)
        => HandleResult(await _service.GetMyFounderSessionsAsync(CurrentUserId, ct));

    [HttpGet("consultant")]
    [Authorize(Roles = "FinancialConsultant")]
    public async Task<IActionResult> GetMineConsultant(CancellationToken ct = default)
        => HandleResult(await _service.GetMyConsultantSessionsAsync(CurrentUserId, ct));

    [HttpPatch("{sessionId:int}/complete")]
    [Authorize(Roles = "FinancialConsultant")]
    public async Task<IActionResult> Complete(int sessionId, [FromBody] CompleteSessionRequest request, CancellationToken ct = default)
        => HandleResult(await _service.CompleteAsync(CurrentUserId, sessionId, request, ct));
}

