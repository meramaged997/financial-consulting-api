using Startawy.Application.DTOs.Requests;
using startawy.Application.Common.Models;
using startawy.Application.DTOs.Responses;

namespace Startawy.Application.Interfaces;

public interface IBudgetService
{
    Task<Result<BudgetAnalysisResponse>> CreateAsync(string userId, CreateBudgetRequest request, CancellationToken ct = default);
    Task<Result<IReadOnlyList<BudgetAnalysisResponse>>> GetHistoryAsync(string userId, CancellationToken ct = default);
    Task<Result<BudgetAnalysisResponse>> GetByIdAsync(string userId, int id, CancellationToken ct = default);
    Task<Result> DeleteAsync(string userId, int id, CancellationToken ct = default);
}
