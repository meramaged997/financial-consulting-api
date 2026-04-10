using startawy.Core.Enums;

namespace startawy.Core.Entities;

public class MarketTrend : BaseEntity
{
    public int            MarketResearchId { get; set; }
    public string         TrendName        { get; set; } = string.Empty;
    public string         Description      { get; set; } = string.Empty;
    public TrendDirection Direction        { get; set; }
    public int            ImpactScore      { get; set; }
}
