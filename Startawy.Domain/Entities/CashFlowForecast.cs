namespace startawy.Core.Entities;

public class CashFlowForecast : AuditableEntity
{
    public string  UserId             { get; set; } = string.Empty;
    public string  BusinessName       { get; set; } = string.Empty;
    public int     ForecastMonths     { get; set; } = 12;
    public decimal InitialCashBalance { get; set; }
    public decimal MonthlyRevenueTrend { get; set; }
    public decimal MonthlyExpenseTrend { get; set; }
    public decimal GrowthRate         { get; set; }
    public string  Insights           { get; set; } = string.Empty;
    public string  GrowthRecommendations { get; set; } = string.Empty;
    public decimal ProjectedRunway    { get; set; }

    public ICollection<MonthlyForecast> MonthlyForecasts { get; set; } = new List<MonthlyForecast>();
}
