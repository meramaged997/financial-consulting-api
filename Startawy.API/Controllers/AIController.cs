using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.Interfaces;

namespace Startawy.API.Controllers;

[ApiController]
[Route("api/ai")]
[Authorize]
public class AIController : BaseController
{
    private readonly IChatService _service;

    public AIController(IChatService service) => _service = service;

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] SendChatMessageRequest request, CancellationToken ct = default)
        => HandleResult(await _service.SendMessageAsync(CurrentUserId, request, ct));

    [HttpGet("sessions")]
    public async Task<IActionResult> GetSessions(CancellationToken ct = default)
        => HandleResult(await _service.GetSessionsAsync(CurrentUserId, ct));

    [HttpGet("sessions/{sessionId:int}")]
    public async Task<IActionResult> GetHistory(int sessionId, CancellationToken ct = default)
        => HandleResult(await _service.GetHistoryAsync(CurrentUserId, sessionId, ct));
}
