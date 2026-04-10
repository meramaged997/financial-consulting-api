using startawy.Application.Common.Models;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.DTOs.Responses;

namespace Startawy.Application.Interfaces;

public interface ISubscriptionService
{
    Task<Result<SubscriptionResponse>> GetMySubscriptionAsync(string userId, CancellationToken ct = default);
    Task<Result<PaymentResponse>> UpgradeAsync(string userId, UpgradePackageRequest request, CancellationToken ct = default);
}

