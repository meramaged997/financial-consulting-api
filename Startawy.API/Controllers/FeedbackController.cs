using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.Interfaces;

namespace Startawy.API.Controllers;

[ApiController]
[Route("api/feedback")]
[Authorize]
public class FeedbackController : BaseController
{
    private readonly IFeedbackService _service;

    public FeedbackController(IFeedbackService service) => _service = service;

    [HttpPost]
    [Authorize(Roles = "StartupFounder")]
    public async Task<IActionResult> Submit([FromBody] CreateFeedbackRequest request, CancellationToken ct = default)
        => HandleResult(await _service.SubmitAsync(CurrentUserId, request, ct));

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
        => HandleResult(await _service.GetAllAsync(ct));

    [HttpPatch("{feedbackId:int}/review")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Review(int feedbackId, [FromBody] ReviewFeedbackRequest request, CancellationToken ct = default)
        => HandleResult(await _service.ReviewAsync(CurrentUserId, feedbackId, request, ct));
}

