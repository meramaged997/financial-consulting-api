using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Startawy.Application.Common;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.Interfaces;

namespace Startawy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "BasicOrAbove")]
public class CashFlowController : BaseController
{
    private readonly ICashFlowService _service;

    public CashFlowController(ICashFlowService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Generate([FromBody] CreateCashFlowForecastRequest request, CancellationToken ct = default)
    {
        var result = await _service.CreateAsync(CurrentUserId, request, ct);
        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created,
                ApiResponse<object>.Ok(result.Value!, "Cash flow forecast created."))
            : BadRequest(ApiResponse<object>.Fail(result.Error ?? "Request failed."));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
        => HandleResult(await _service.GetAllAsync(CurrentUserId, ct));
}
