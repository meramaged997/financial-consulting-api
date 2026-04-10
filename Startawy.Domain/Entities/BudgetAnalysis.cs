using startawy.Core.Enums;

namespace startawy.Core.Entities;

public class BudgetAnalysis : AuditableEntity
{
    public string  UserId          { get; set; } = string.Empty;
    public string? PackageId       { get; set; }
    public string  BusinessName    { get; set; } = string.Empty;
    public string  Industry        { get; set; } = string.Empty;
    public string  Period          { get; set; } = string.Empty;
    public decimal TotalRevenue    { get; set; }
    public decimal FixedCosts      { get; set; }
    public decimal VariableCosts   { get; set; }
    public decimal TotalExpenses   { get; set; }
    public decimal NetProfit       => TotalRevenue - TotalExpenses;
    public double  ProfitMargin    => TotalRevenue != 0 ? (double)((TotalRevenue - TotalExpenses) / TotalRevenue) * 100 : 0;
    public string  ProfitStatus    { get; set; } = "Break-even";
    public string  Recommendations { get; set; } = string.Empty;
    public string  OptimizationPlan { get; set; } = string.Empty;
    public RiskLevel RiskLevel     { get; set; }
    public DateTime AnalysisDate   { get; set; } = DateTime.UtcNow;

    public ICollection<BudgetLineItem> LineItems { get; set; } = new List<BudgetLineItem>();
}
