using startawy.Core.Entities;
using startawy.Core.Interfaces.Repositories;
using startawy.Application.Common.Models;
using Startawy.Application.DTOs.Requests;
using startawy.Application.DTOs.Responses;
using Startawy.Application.Interfaces;

namespace Startawy.Application.Services;

public class CashFlowService : ICashFlowService
{
    private readonly ICashFlowRepository _cashFlowRepo;

    public CashFlowService(ICashFlowRepository cashFlowRepo) => _cashFlowRepo = cashFlowRepo;

    public async Task<Result<CashFlowForecastResponse>> CreateAsync(string userId, CreateCashFlowForecastRequest request, CancellationToken ct = default)
    {
        var months = request.ForecastMonths;
        if (months < 3) months = 3;
        if (months > 6) months = 6;

        var entity = new CashFlowForecast
        {
            UserId = userId,
            BusinessName = request.BusinessName,
            InitialCashBalance = request.InitialCashBalance,
            MonthlyRevenueTrend = request.MonthlyRevenueTrend,
            MonthlyExpenseTrend = request.MonthlyExpenseTrend,
            GrowthRate = request.GrowthRate,
            ForecastMonths = months,
            Insights = string.Empty,
            GrowthRecommendations = string.Empty,
            ProjectedRunway = request.InitialCashBalance,
            CreatedBy = userId
        };

        // Business rule: 3–6 month forecast; NetCashFlow = Inflow - Outflow; warn if any future value is negative.
        var now = DateTime.UtcNow;
        decimal cash = entity.InitialCashBalance;
        var anyNegative = false;
        for (var i = 0; i < months; i++)
        {
            // Simple trend model: apply growth rate compounded monthly on revenue; expenses follow expense trend.
            var revenue = request.MonthlyRevenueTrend * (decimal)Math.Pow((double)(1m + request.GrowthRate), i);
            var expenses = request.MonthlyExpenseTrend;
            var net = revenue - expenses;
            cash += net;

            if (net < 0 || cash < 0) anyNegative = true;

            var date = now.AddMonths(i + 1);
            entity.MonthlyForecasts.Add(new MonthlyForecast
            {
                Month = date.Month,
                Year = date.Year,
                ProjectedRevenue = decimal.Round(revenue, 2),
                ProjectedExpenses = decimal.Round(expenses, 2),
                CumulativeCashBalance = decimal.Round(cash, 2),
                ConfidenceScore = 0.65 // placeholder heuristic; can be improved with historical data
            });
        }

        entity.ProjectedRunway = cash;
        entity.Insights = anyNegative
            ? "Warning: projected net cash flow becomes negative in the forecast period. Consider reducing expenses, improving collections, or securing additional funding."
            : "Cash flow forecast looks healthy for the selected period. Continue monitoring inflow/outflow and maintain a cash buffer.";
        entity.GrowthRecommendations = anyNegative
            ? "Focus on improving operating efficiency, controlling burn rate, and increasing revenue reliability (subscriptions, contracts)."
            : "Consider reinvesting surplus cash into growth while keeping a 3–6 month runway buffer.";

        var added = await _cashFlowRepo.AddAsync(entity, ct);
        var withMonthly = await _cashFlowRepo.GetWithMonthlyDataAsync(added.Id, ct);
        return Result<CashFlowForecastResponse>.Success(MapToResponse(withMonthly ?? added));
    }

    public async Task<Result<IReadOnlyList<CashFlowForecastResponse>>> GetAllAsync(string userId, CancellationToken ct = default)
    {
        var list = await _cashFlowRepo.GetByUserAsync(userId, ct);
        return Result<IReadOnlyList<CashFlowForecastResponse>>.Success(list.Select(MapToResponse).ToList());
    }

    private static CashFlowForecastResponse MapToResponse(CashFlowForecast c)
    {
        var monthly = (c.MonthlyForecasts ?? Array.Empty<MonthlyForecast>())
            .OrderBy(m => m.Year).ThenBy(m => m.Month)
            .Select(m => new MonthlyForecastResponse(
                m.Month, m.Year, m.ProjectedRevenue, m.ProjectedExpenses,
                m.ProjectedRevenue - m.ProjectedExpenses, m.CumulativeCashBalance, m.ConfidenceScore
            )).ToList();
        return new CashFlowForecastResponse(
            c.Id, c.BusinessName, c.ForecastMonths, c.InitialCashBalance,
            monthly, c.Insights, c.GrowthRecommendations, c.ProjectedRunway, c.CreatedAt
        );
    }
}
