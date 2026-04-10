using startawy.Core.Enums;

namespace startawy.Application.DTOs.Responses;

public record FinancialStatementResponse(
    int          Id,
    StatementType Type,
    string        Period,
    decimal       GrossRevenue,
    decimal       GrossProfit,
    decimal       OperatingIncome,
    decimal       NetIncome,
    decimal       TotalAssets,
    decimal       TotalLiabilities,
    decimal       Equity,
    decimal       NetCashFlow,
    string        AnalysisNotes,
    string        PerformanceForecast,
    RiskLevel     RiskAssessment,
    DateTime      CreatedAt
);
