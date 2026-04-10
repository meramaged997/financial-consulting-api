using startawy.Core.Enums;

namespace startawy.Core.Entities;

public class FinancialStatement : AuditableEntity
{
    public string        UserId               { get; set; } = string.Empty;
    public StatementType Type                 { get; set; }
    public string        Period               { get; set; } = string.Empty;
    public DateTime      StatementDate        { get; set; }

    // Income Statement
    public decimal GrossRevenue      { get; set; }
    public decimal CostOfGoodsSold   { get; set; }
    public decimal GrossProfit       => GrossRevenue - CostOfGoodsSold;
    public decimal OperatingExpenses { get; set; }
    public decimal OperatingIncome   => GrossProfit - OperatingExpenses;
    public decimal NetIncome         { get; set; }

    // Balance Sheet
    public decimal TotalAssets      { get; set; }
    public decimal TotalLiabilities { get; set; }
    public decimal Equity           => TotalAssets - TotalLiabilities;

    // Cash Flow
    public decimal OperatingCashFlow { get; set; }
    public decimal InvestingCashFlow { get; set; }
    public decimal FinancingCashFlow { get; set; }
    public decimal NetCashFlow       => OperatingCashFlow + InvestingCashFlow + FinancingCashFlow;

    public string    AnalysisNotes       { get; set; } = string.Empty;
    public string    PerformanceForecast { get; set; } = string.Empty;
    public RiskLevel RiskAssessment      { get; set; }
}
