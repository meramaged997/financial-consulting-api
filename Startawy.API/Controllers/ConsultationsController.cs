using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Startawy.Application.Common;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.Interfaces;

namespace Startawy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConsultationsController : BaseController
{
    private readonly IConsultationService _service;

    public ConsultationsController(IConsultationService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateConsultationRequest request, CancellationToken ct = default)
    {
        var result = await _service.CreateAsync(CurrentUserId, request, ct);
        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created,
                ApiResponse<object>.Ok(result.Value!, "Consultation request created."))
            : BadRequest(ApiResponse<object>.Fail(result.Error ?? "Request failed."));
    }

    [HttpGet("mine")]
    public async Task<IActionResult> GetMine(CancellationToken ct = default)
        => HandleResult(await _service.GetMineAsync(CurrentUserId, ct));

    [HttpGet("all")]
    [Authorize(Roles = "FinancialConsultant,Administrator")]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
        => HandleResult(await _service.GetAllAsync(ct));

    [HttpPatch("{id:int}/status")]
    [Authorize(Roles = "FinancialConsultant,Administrator")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateConsultationStatusRequest request, CancellationToken ct = default)
        => HandleResult(await _service.UpdateStatusAsync(id, request, ct));
}
