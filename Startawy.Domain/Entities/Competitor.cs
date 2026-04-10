namespace startawy.Core.Entities;

public class Competitor : BaseEntity
{
    public int    MarketResearchId    { get; set; }
    public string Name               { get; set; } = string.Empty;
    public string Website            { get; set; } = string.Empty;
    public string Strengths          { get; set; } = string.Empty;
    public string Weaknesses         { get; set; } = string.Empty;
    public double MarketShareEstimate { get; set; }
}
