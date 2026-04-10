using startawy.Core.Entities;
using startawy.Core.Enums;
using startawy.Core.Interfaces.Repositories;
using startawy.Application.Common.Models;
using Startawy.Application.DTOs.Requests;
using startawy.Application.DTOs.Responses;
using Startawy.Application.Interfaces;

namespace Startawy.Application.Services;

public class BudgetService : IBudgetService
{
    private readonly IBudgetRepository _budgetRepo;

    public BudgetService(IBudgetRepository budgetRepo) => _budgetRepo = budgetRepo;

    public async Task<Result<BudgetAnalysisResponse>> CreateAsync(string userId, CreateBudgetRequest request, CancellationToken ct = default)
    {
        var totalRevenue = request.LineItems.Where(l => l.Type == LineItemType.Revenue).Sum(l => l.ActualAmount);

        // Business rule: TotalCosts = FixedCosts + VariableCosts, NetProfit = Revenue - TotalCosts
        // We infer Fixed/Variable from the line item category (e.g. "Fixed", "Variable"). If not provided, treat as Variable.
        static bool IsFixed(string category)
            => category?.Trim().Equals("Fixed", StringComparison.OrdinalIgnoreCase) == true;

        var fixedCosts = request.LineItems
            .Where(l => l.Type == LineItemType.Expense && IsFixed(l.Category))
            .Sum(l => l.ActualAmount);

        var variableCosts = request.LineItems
            .Where(l => l.Type == LineItemType.Expense && !IsFixed(l.Category))
            .Sum(l => l.ActualAmount);

        var totalExpenses = fixedCosts + variableCosts;
        var netProfit = totalRevenue - totalExpenses;

        var status = netProfit > 0 ? "Profitable" : netProfit < 0 ? "Loss" : "Break-even";

        var profitMargin = totalRevenue != 0 ? (netProfit / totalRevenue) * 100m : 0m;
        var rec = BuildRecommendations(status, profitMargin);
        var plan = BuildOptimizationPlan(status);

        var analysis = new BudgetAnalysis
        {
            UserId = userId,
            BusinessName = request.BusinessName,
            Industry = request.Industry,
            Period = request.Period,
            TotalRevenue = totalRevenue,
            FixedCosts = fixedCosts,
            VariableCosts = variableCosts,
            TotalExpenses = totalExpenses,
            ProfitStatus = status,
            Recommendations = rec,
            OptimizationPlan = plan,
            RiskLevel = netProfit < 0 ? RiskLevel.High : profitMargin < 10 ? RiskLevel.Medium : RiskLevel.Low,
            AnalysisDate = DateTime.UtcNow,
            CreatedBy = userId
        };
        foreach (var item in request.LineItems)
        {
            analysis.LineItems.Add(new BudgetLineItem
            {
                Category = item.Category,
                Description = item.Description,
                PlannedAmount = item.PlannedAmount,
                ActualAmount = item.ActualAmount,
                Type = item.Type,
                CreatedAt = DateTime.UtcNow
            });
        }
        var added = await _budgetRepo.AddAsync(analysis, ct);
        return Result<BudgetAnalysisResponse>.Success(MapToResponse(added));
    }

    public async Task<Result<IReadOnlyList<BudgetAnalysisResponse>>> GetHistoryAsync(string userId, CancellationToken ct = default)
    {
        var list = await _budgetRepo.GetByUserAsync(userId, ct);
        return Result<IReadOnlyList<BudgetAnalysisResponse>>.Success(list.Select(MapToResponse).ToList());
    }

    public async Task<Result<BudgetAnalysisResponse>> GetByIdAsync(string userId, int id, CancellationToken ct = default)
    {
        var entity = await _budgetRepo.GetWithLineItemsAsync(id, ct);
        if (entity is null) return Result<BudgetAnalysisResponse>.Failure("Budget analysis not found.");
        if (entity.UserId != userId) return Result<BudgetAnalysisResponse>.Failure("Not authorized.");
        return Result<BudgetAnalysisResponse>.Success(MapToResponse(entity));
    }

    public async Task<Result> DeleteAsync(string userId, int id, CancellationToken ct = default)
    {
        var entity = await _budgetRepo.GetByIdAsync(id, ct);
        if (entity is null) return Result.Failure("Budget analysis not found.");
        if (entity.UserId != userId) return Result.Failure("Not authorized.");
        await _budgetRepo.DeleteAsync(entity, ct);
        return Result.Success();
    }

    private static BudgetAnalysisResponse MapToResponse(BudgetAnalysis b)
    {
        return new BudgetAnalysisResponse(
            b.Id,
            b.BusinessName,
            b.Industry,
            b.Period,
            b.TotalRevenue,
            b.FixedCosts,
            b.VariableCosts,
            b.TotalExpenses,
            b.NetProfit,
            (decimal)b.ProfitMargin,
            b.ProfitStatus,
            b.RiskLevel,
            b.Recommendations,
            b.OptimizationPlan,
            b.LineItems.Select(li => new BudgetLineItemResponse(li.Id, li.Category, li.Description, li.PlannedAmount, li.ActualAmount, li.ActualAmount - li.PlannedAmount, li.Type)).ToList(),
            b.AnalysisDate
        );
    }

    private static string BuildRecommendations(string status, decimal profitMargin)
    {
        return status switch
        {
            "Loss" => "Your business is currently operating at a loss. Consider reducing costs (especially fixed costs) and/or increasing revenue. Review your largest expense categories and test pricing or sales channel improvements.",
            "Break-even" => "You are at break-even. Focus on improving margins by optimizing operational costs and increasing revenue through better conversion, pricing, or product mix.",
            "Profitable" when profitMargin < 10 => "You are profitable, but margins are low. Prioritize cost optimization, renegotiate supplier contracts, and focus on higher-margin offerings.",
            "Profitable" when profitMargin >= 25 => "You are highly profitable. Consider expansion opportunities, reinvestment in growth (marketing/sales), and building a cash buffer to reduce risk.",
            "Profitable" => "You are profitable. Keep monitoring costs, strengthen cash reserves, and invest in sustainable growth initiatives.",
            _ => "Review your inputs and iterate to improve financial performance."
        };
    }

    private static string BuildOptimizationPlan(string status)
    {
        return status switch
        {
            "Loss" => "1) Identify top 3 expense drivers. 2) Reduce fixed commitments where possible. 3) Set revenue targets per channel. 4) Re-forecast monthly and track variance vs plan.",
            "Break-even" => "1) Improve unit economics (COGS and operating efficiency). 2) Increase AR collection speed. 3) Test pricing/packaging. 4) Track monthly margins and cash runway.",
            "Profitable" => "1) Build a 3–6 month cash buffer. 2) Allocate budget to growth experiments. 3) Monitor burn rate and CAC/LTV. 4) Re-run analysis each period for trend tracking.",
            _ => string.Empty
        };
    }
}
