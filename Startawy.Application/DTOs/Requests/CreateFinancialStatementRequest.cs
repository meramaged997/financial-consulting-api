using startawy.Core.Enums;

namespace Startawy.Application.DTOs.Requests;

public record CreateFinancialStatementRequest(
    StatementType Type,
    string        Period,
    DateTime      StatementDate,
    decimal       GrossRevenue,
    decimal       CostOfGoodsSold,
    decimal       OperatingExpenses,
    decimal       NetIncome,
    decimal       TotalAssets,
    decimal       TotalLiabilities,
    decimal       OperatingCashFlow,
    decimal       InvestingCashFlow,
    decimal       FinancingCashFlow
);
