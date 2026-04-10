using startawy.Core.Enums;

namespace startawy.Application.DTOs.Responses;

public record BudgetAnalysisResponse(
    int      Id,
    string   BusinessName,
    string   Industry,
    string   Period,
    decimal  TotalRevenue,
    decimal  FixedCosts,
    decimal  VariableCosts,
    decimal  TotalExpenses,
    decimal  NetProfit,
    decimal  ProfitMargin,
    string   ProfitStatus,
    RiskLevel RiskLevel,
    string   Recommendations,
    string   OptimizationPlan,
    List<BudgetLineItemResponse> LineItems,
    DateTime AnalysisDate
);

public record BudgetLineItemResponse(
    int         Id,
    string      Category,
    string      Description,
    decimal     PlannedAmount,
    decimal     ActualAmount,
    decimal     Variance,
    LineItemType Type
);
