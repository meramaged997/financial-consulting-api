namespace startawy.Application.DTOs.Responses;

public record CashFlowForecastResponse(
    int      Id,
    string   BusinessName,
    int      ForecastMonths,
    decimal  InitialCashBalance,
    List<MonthlyForecastResponse> MonthlyForecasts,
    string   Insights,
    string   GrowthRecommendations,
    decimal  ProjectedRunway,
    DateTime CreatedAt
);

public record MonthlyForecastResponse(
    int     Month,
    int     Year,
    decimal ProjectedRevenue,
    decimal ProjectedExpenses,
    decimal ProjectedNetCashFlow,
    decimal CumulativeCashBalance,
    double  ConfidenceScore
);
