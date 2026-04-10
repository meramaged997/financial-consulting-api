using Startawy.Application.DTOs.Requests;
using startawy.Application.Common.Models;
using startawy.Application.DTOs.Responses;

namespace Startawy.Application.Interfaces;

public interface ICashFlowService
{
    Task<Result<CashFlowForecastResponse>> CreateAsync(string userId, CreateCashFlowForecastRequest request, CancellationToken ct = default);
    Task<Result<IReadOnlyList<CashFlowForecastResponse>>> GetAllAsync(string userId, CancellationToken ct = default);
}
