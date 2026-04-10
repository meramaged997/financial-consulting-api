using Startawy.Application.DTOs.Requests;
using startawy.Application.Common.Models;
using startawy.Application.DTOs.Responses;
using startawy.Core.Enums;

namespace Startawy.Application.Interfaces;

public interface IFinancialService
{
    Task<Result<FinancialStatementResponse>> CreateAsync(string userId, CreateFinancialStatementRequest request, CancellationToken ct = default);
    Task<Result<IReadOnlyList<FinancialStatementResponse>>> GetAllAsync(string userId, StatementType? type, CancellationToken ct = default);
    Task<Result<FinancialStatementResponse>> GetRiskAnalysisAsync(string userId, int id, CancellationToken ct = default);
}
