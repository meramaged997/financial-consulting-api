namespace startawy.Core.Entities;

public class MarketResearch : AuditableEntity
{
    public string  UserId              { get; set; } = string.Empty;
    public string  Industry            { get; set; } = string.Empty;
    public string  TargetMarket        { get; set; } = string.Empty;
    public string  GeographicScope     { get; set; } = string.Empty;
    public decimal EstimatedMarketSize { get; set; }
    public double  MarketGrowthRate    { get; set; }
    public string  CompetitorAnalysis  { get; set; } = string.Empty;
    public string  TrendAnalysis       { get; set; } = string.Empty;
    public string  OpportunityInsights { get; set; } = string.Empty;
    public string  GeneratedReport     { get; set; } = string.Empty;

    public ICollection<Competitor>  Competitors { get; set; } = new List<Competitor>();
    public ICollection<MarketTrend> Trends      { get; set; } = new List<MarketTrend>();
}
