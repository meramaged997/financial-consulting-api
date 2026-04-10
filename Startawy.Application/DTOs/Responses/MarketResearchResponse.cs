using startawy.Core.Enums;

namespace startawy.Application.DTOs.Responses;

public record MarketResearchResponse(
    int      Id,
    string   Industry,
    string   TargetMarket,
    string   GeographicScope,
    decimal  EstimatedMarketSize,
    double   MarketGrowthRate,
    string   CompetitorAnalysis,
    string   TrendAnalysis,
    string   OpportunityInsights,
    string   GeneratedReport,
    List<CompetitorResponse>  Competitors,
    List<MarketTrendResponse> Trends,
    DateTime CreatedAt
);

public record CompetitorResponse(
    int    Id,
    string Name,
    string Website,
    string Strengths,
    string Weaknesses,
    double MarketShareEstimate
);

public record MarketTrendResponse(
    int           Id,
    string        TrendName,
    string        Description,
    TrendDirection Direction,
    int           ImpactScore
);
