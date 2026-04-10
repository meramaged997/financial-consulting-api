using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Startawy.Application.Common;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.Interfaces;

namespace Startawy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BudgetController : BaseController
{
    private readonly IBudgetService _service;

    public BudgetController(IBudgetService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Analyze([FromBody] CreateBudgetRequest request, CancellationToken ct = default)
    {
        var result = await _service.CreateAsync(CurrentUserId, request, ct);
        if (!result.IsSuccess)
            return BadRequest(ApiResponse<object>.Fail(result.Error ?? "Request failed."));

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<object>.Ok(result.Value!, "Budget analysis created."));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
        => HandleResult(await _service.GetHistoryAsync(CurrentUserId, ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
        => HandleResult(await _service.GetByIdAsync(CurrentUserId, id, ct));

    // Covers: /generate-budget-report (frontend action)
    // Returns the budget analysis details; frontend can render/export PDF.
    [HttpGet("{id:int}/report")]
    public async Task<IActionResult> GetReport(int id, CancellationToken ct = default)
        => HandleResult(await _service.GetByIdAsync(CurrentUserId, id, ct));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        => HandleResult(await _service.DeleteAsync(CurrentUserId, id, ct));
}
