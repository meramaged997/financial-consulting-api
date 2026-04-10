using startawy.Application.Common.Models;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.DTOs.Responses;

namespace Startawy.Application.Interfaces;

public interface IPaymentService
{
    Task<Result<PaymentIntentResponse>> CreateIntentAsync(string userId, CreatePaymentIntentRequest request, string? idempotencyKey, CancellationToken ct = default);
    Task<Result<object>> ConfirmAsync(string userId, string transactionId, ConfirmPaymentRequest request, CancellationToken ct = default);
}

