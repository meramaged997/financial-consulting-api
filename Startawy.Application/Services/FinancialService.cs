using startawy.Core.Entities;
using startawy.Core.Enums;
using startawy.Core.Interfaces.Repositories;
using startawy.Application.Common.Models;
using Startawy.Application.DTOs.Requests;
using startawy.Application.DTOs.Responses;
using Startawy.Application.Interfaces;

namespace Startawy.Application.Services;

public class FinancialService : IFinancialService
{
    private readonly IFinancialRepository _financialRepo;

    public FinancialService(IFinancialRepository financialRepo) => _financialRepo = financialRepo;

    public async Task<Result<FinancialStatementResponse>> CreateAsync(string userId, CreateFinancialStatementRequest request, CancellationToken ct = default)
    {
        var entity = new FinancialStatement
        {
            UserId = userId,
            Type = request.Type,
            Period = request.Period,
            StatementDate = request.StatementDate,
            GrossRevenue = request.GrossRevenue,
            CostOfGoodsSold = request.CostOfGoodsSold,
            OperatingExpenses = request.OperatingExpenses,
            NetIncome = request.NetIncome,
            TotalAssets = request.TotalAssets,
            TotalLiabilities = request.TotalLiabilities,
            OperatingCashFlow = request.OperatingCashFlow,
            InvestingCashFlow = request.InvestingCashFlow,
            FinancingCashFlow = request.FinancingCashFlow,
            AnalysisNotes = string.Empty,
            PerformanceForecast = string.Empty,
            RiskAssessment = RiskLevel.Medium,
            CreatedBy = userId
        };
        var added = await _financialRepo.AddAsync(entity, ct);
        return Result<FinancialStatementResponse>.Success(MapToResponse(added));
    }

    public async Task<Result<IReadOnlyList<FinancialStatementResponse>>> GetAllAsync(string userId, StatementType? type, CancellationToken ct = default)
    {
        var list = await _financialRepo.GetByUserAsync(userId, type, ct);
        return Result<IReadOnlyList<FinancialStatementResponse>>.Success(list.Select(MapToResponse).ToList());
    }

    public async Task<Result<FinancialStatementResponse>> GetRiskAnalysisAsync(string userId, int id, CancellationToken ct = default)
    {
        var entity = await _financialRepo.GetByIdAsync(id, ct);
        if (entity is null) return Result<FinancialStatementResponse>.Failure("Statement not found.");
        if (entity.UserId != userId) return Result<FinancialStatementResponse>.Failure("Not authorized.");
        return Result<FinancialStatementResponse>.Success(MapToResponse(entity));
    }

    private static FinancialStatementResponse MapToResponse(FinancialStatement f)
    {
        return new FinancialStatementResponse(
            f.Id, f.Type, f.Period,
            f.GrossRevenue, f.GrossProfit, f.OperatingIncome, f.NetIncome,
            f.TotalAssets, f.TotalLiabilities, f.Equity, f.NetCashFlow,
            f.AnalysisNotes, f.PerformanceForecast, f.RiskAssessment, f.CreatedAt
        );
    }
}
