using startawy.Core.Enums;

namespace startawy.Core.Entities;

public class BudgetLineItem : BaseEntity
{
    public int          BudgetAnalysisId { get; set; }
    public BudgetAnalysis BudgetAnalysis { get; set; } = null!;
    public string       Category         { get; set; } = string.Empty;
    public string       Description      { get; set; } = string.Empty;
    public decimal      PlannedAmount    { get; set; }
    public decimal      ActualAmount     { get; set; }
    public decimal      Variance         => ActualAmount - PlannedAmount;
    public LineItemType Type             { get; set; }
}
