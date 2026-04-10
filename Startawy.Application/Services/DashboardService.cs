using startawy.Core.Interfaces.Repositories;
using startawy.Application.Common.Models;
using startawy.Application.DTOs.Responses;
using Startawy.Application.Interfaces;
using startawy.Core.Entities;

namespace Startawy.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepo;
    private readonly IBudgetRepository _budgetRepo;
    private readonly ICashFlowRepository _cashFlowRepo;

    public DashboardService(
        IDashboardRepository dashboardRepo,
        IBudgetRepository budgetRepo,
        ICashFlowRepository cashFlowRepo)
    {
        _dashboardRepo = dashboardRepo;
        _budgetRepo = budgetRepo;
        _cashFlowRepo = cashFlowRepo;
    }

    public async Task<Result<DashboardResponse>> GetAsync(string userId, CancellationToken ct = default)
    {
        // Auto-generate a snapshot if none exist for the current month.
        var monthKey = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var existingThisMonth = await _dashboardRepo.ExistsAsync(d =>
            d.UserId == userId &&
            d.SnapshotDate.Year == monthKey.Year &&
            d.SnapshotDate.Month == monthKey.Month, ct);

        if (!existingThisMonth)
        {
            var latestBudget = (await _budgetRepo.GetByUserAsync(userId, ct)).FirstOrDefault();
            var latestCash = await _cashFlowRepo.GetLatestByUserAsync(userId, ct);

            var revenue = latestBudget?.TotalRevenue ?? 0m;
            var expenses = latestBudget?.TotalExpenses ?? 0m;
            var net = revenue - expenses;

            decimal cashBalance = 0m;
            if (latestCash?.MonthlyForecasts is { Count: > 0 })
                cashBalance = latestCash.MonthlyForecasts.OrderBy(m => m.Year).ThenBy(m => m.Month).Last().CumulativeCashBalance;
            else
                cashBalance = latestCash?.InitialCashBalance ?? 0m;

            var burnRate = net < 0 ? (double)(-net) : 0d;
            var insights = BuildInsights(latestBudget, latestCash, net, cashBalance);

            await _dashboardRepo.AddAsync(new DashboardSnapshot
            {
                UserId = userId,
                SnapshotDate = DateTime.UtcNow,
                Revenue = revenue,
                Expenses = expenses,
                NetProfit = net,
                CashBalance = cashBalance,
                BurnRate = burnRate,
                CustomerCount = 0,
                CustomerAcquisitionCost = 0,
                CustomerLifetimeValue = 0,
                ChurnRate = 0,
                PredictiveInsights = insights
            }, ct);
        }

        var snapshots = await _dashboardRepo.GetRecentByUserAsync(userId, 6, ct);
        if (snapshots.Count == 0)
        {
            return Result<DashboardResponse>.Success(new DashboardResponse(
                0, 0, 0, 0, 0, 0, 0, 0, 0, string.Empty,
                new List<ChartPoint>(), new List<ChartPoint>()
            ));
        }
        var latest = snapshots[0];
        var revenueHistory = snapshots.Take(6).Select(s => new ChartPoint(s.SnapshotDate.ToString("yyyy-MM"), s.Revenue)).ToList();
        var expenseHistory = snapshots.Take(6).Select(s => new ChartPoint(s.SnapshotDate.ToString("yyyy-MM"), s.Expenses)).ToList();
        var response = new DashboardResponse(
            latest.Revenue,
            latest.Expenses,
            latest.NetProfit,
            latest.CashBalance,
            latest.BurnRate,
            latest.CustomerCount,
            latest.CustomerAcquisitionCost,
            latest.CustomerLifetimeValue,
            latest.ChurnRate,
            latest.PredictiveInsights,
            revenueHistory,
            expenseHistory
        );
        return Result<DashboardResponse>.Success(response);
    }

    private static string BuildInsights(BudgetAnalysis? budget, CashFlowForecast? cash, decimal netProfit, decimal cashBalance)
    {
        var profitStatus = budget?.ProfitStatus ?? (netProfit > 0 ? "Profitable" : netProfit < 0 ? "Loss" : "Break-even");
        var cashWarning = cash?.Insights ?? string.Empty;

        if (profitStatus == "Loss" && cashBalance < 0)
            return "Critical: you're operating at a loss and projected cash balance is negative. Reduce burn immediately and consider funding options.";

        if (profitStatus == "Loss")
            return "You are operating at a loss. Focus on reducing costs and increasing revenue. Track burn rate closely.";

        if (profitStatus == "Break-even")
            return "You are at break-even. Improve margins through cost optimization and revenue growth experiments.";

        if (!string.IsNullOrWhiteSpace(cashWarning) && cashWarning.Contains("Warning", StringComparison.OrdinalIgnoreCase))
            return "You are profitable, but cash flow risk is detected in the forecast. Improve collections and control expenses.";

        return "Your performance looks healthy. Maintain a cash buffer and invest carefully in sustainable growth.";
    }
}
