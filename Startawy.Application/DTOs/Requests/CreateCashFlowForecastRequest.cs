namespace Startawy.Application.DTOs.Requests;

public record CreateCashFlowForecastRequest(
    string  BusinessName,
    decimal InitialCashBalance,
    decimal MonthlyRevenueTrend,
    decimal MonthlyExpenseTrend,
    decimal GrowthRate,
    int     ForecastMonths = 12
);
