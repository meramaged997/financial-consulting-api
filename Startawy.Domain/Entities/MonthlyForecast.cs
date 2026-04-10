namespace startawy.Core.Entities;

public class MonthlyForecast : BaseEntity
{
    public int     CashFlowForecastId    { get; set; }
    public int     Month                 { get; set; }
    public int     Year                  { get; set; }
    public decimal ProjectedRevenue      { get; set; }
    public decimal ProjectedExpenses     { get; set; }
    public decimal ProjectedNetCashFlow  => ProjectedRevenue - ProjectedExpenses;
    public decimal CumulativeCashBalance { get; set; }
    public double  ConfidenceScore       { get; set; }
}
