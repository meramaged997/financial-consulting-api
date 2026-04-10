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
public class FinancialController : BaseController
{
    private readonly IFinancialService _service;

    public FinancialController(IFinancialService service) => _service = service;

    [HttpPost("statements")]
    public async Task<IActionResult> Generate([FromBody] CreateFinancialStatementRequest request, CancellationToken ct = default)
    {
        var result = await _service.CreateAsync(CurrentUserId, request, ct);
        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created,
                ApiResponse<object>.Ok(result.Value!, "Financial statement created."))
            : BadRequest(ApiResponse<object>.Fail(result.Error ?? "Request failed."));
    }

    [HttpGet("statements")]
    public async Task<IActionResult> GetAll([FromQuery] StatementType? type, CancellationToken ct = default)
        => HandleResult(await _service.GetAllAsync(CurrentUserId, type, ct));

    [HttpGet("statements/{id:int}/risk")]
    public async Task<IActionResult> GetRiskAnalysis(int id, CancellationToken ct = default)
        => HandleResult(await _service.GetRiskAnalysisAsync(CurrentUserId, id, ct));
}
