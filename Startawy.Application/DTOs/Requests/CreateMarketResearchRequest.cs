namespace Startawy.Application.DTOs.Requests;

public record CreateMarketResearchRequest(
    string       Industry,
    string       TargetMarket,
    string       GeographicScope,
    List<string> Keywords,
    List<string>? KnownCompetitors
);
