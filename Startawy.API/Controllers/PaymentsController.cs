using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.DTOs.Responses;
using Startawy.Application.Interfaces;
using Startawy.Infrastructure.Data;

namespace Startawy.API.Controllers;

[ApiController]
[Route("api/payments")]
[Authorize]
public class PaymentsController : BaseController
{
    private readonly IPaymentService _service;
    private readonly AppDbContext _db;

    public PaymentsController(IPaymentService service, AppDbContext db)
    {
        _service = service;
        _db = db;
    }

    [HttpPost("intent")]
    public async Task<IActionResult> CreateIntent([FromBody] CreatePaymentIntentRequest request, CancellationToken ct = default)
    {
        var idempotencyKey = Request.Headers.TryGetValue("Idempotency-Key", out var key) ? key.ToString() : null;
        return HandleResult(await _service.CreateIntentAsync(CurrentUserId, request, idempotencyKey, ct));
    }

    [HttpPost("{transactionId}/confirm")]
    public async Task<IActionResult> Confirm(string transactionId, [FromBody] ConfirmPaymentRequest request, CancellationToken ct = default)
        => HandleResult(await _service.ConfirmAsync(CurrentUserId, transactionId, request, ct));

    // Covers: /my-payments
    [HttpGet("mine")]
    public async Task<IActionResult> GetMine(CancellationToken ct = default)
    {
        var items = await _db.Transactions.AsNoTracking()
            .Where(t => t.UserId == CurrentUserId)
            .OrderByDescending(t => t.TransDate)
            .Select(t => new PaymentResponse(
                t.TransId,
                t.Amount,
                t.Type ?? string.Empty,
                t.PaymentMethod ?? string.Empty,
                t.TransDate,
                t.SubsId ?? string.Empty,
                string.Empty
            ))
            .ToListAsync(ct);

        return Ok(Startawy.Application.Common.ApiResponse<object>.Ok(items));
    }

    // Covers: /add-payment-method (placeholder until a payment-provider vault is added)
    [HttpGet("methods")]
    public IActionResult GetPaymentMethods()
        => Ok(Startawy.Application.Common.ApiResponse<object>.Ok(Array.Empty<object>(), "No saved payment methods (demo)."));

    [HttpPost("methods")]
    public IActionResult AddPaymentMethod()
        => Ok(Startawy.Application.Common.ApiResponse<object>.Ok(new { }, "Payment method added (demo)."));
}

