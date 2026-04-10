namespace startawy.Application.DTOs.Responses;

public record DashboardResponse(
    decimal  Revenue,
    decimal  Expenses,
    decimal  NetProfit,
    decimal  CashBalance,
    double   BurnRate,
    int      CustomerCount,
    double   CustomerAcquisitionCost,
    double   CustomerLifetimeValue,
    double   ChurnRate,
    string   PredictiveInsights,
    List<ChartPoint> RevenueHistory,
    List<ChartPoint> ExpenseHistory
);

public record ChartPoint(string Label, decimal Value);
