using startawy.Core.Entities;
using startawy.Core.Interfaces.Repositories;
using startawy.Application.Common.Models;
using Startawy.Application.DTOs.Requests;
using startawy.Application.DTOs.Responses;
using Startawy.Application.Interfaces;

namespace Startawy.Application.Services;

public class MarketResearchService : IMarketResearchService
{
    private readonly IMarketResearchRepository _marketResearchRepo;

    public MarketResearchService(IMarketResearchRepository marketResearchRepo) => _marketResearchRepo = marketResearchRepo;

    public async Task<Result<MarketResearchResponse>> CreateAsync(string userId, CreateMarketResearchRequest request, CancellationToken ct = default)
    {
        var entity = new MarketResearch
        {
            UserId = userId,
            Industry = request.Industry,
            TargetMarket = request.TargetMarket,
            GeographicScope = request.GeographicScope,
            EstimatedMarketSize = 0,
            MarketGrowthRate = 0,
            CompetitorAnalysis = string.Empty,
            TrendAnalysis = string.Empty,
            OpportunityInsights = string.Empty,
            GeneratedReport = string.Empty,
            CreatedBy = userId
        };
        var added = await _marketResearchRepo.AddAsync(entity, ct);
        var withDetails = await _marketResearchRepo.GetWithDetailsAsync(added.Id, ct);
        return Result<MarketResearchResponse>.Success(MapToResponse(withDetails ?? added));
    }

    public async Task<Result<IReadOnlyList<MarketResearchResponse>>> GetAllAsync(string userId, CancellationToken ct = default)
    {
        var list = await _marketResearchRepo.GetByUserAsync(userId, ct);
        return Result<IReadOnlyList<MarketResearchResponse>>.Success(list.Select(MapToResponse).ToList());
    }

    public async Task<Result<MarketResearchResponse>> GetByIdAsync(string userId, int id, CancellationToken ct = default)
    {
        var entity = await _marketResearchRepo.GetWithDetailsAsync(id, ct);
        if (entity is null) return Result<MarketResearchResponse>.Failure("Market research not found.");
        if (entity.UserId != userId) return Result<MarketResearchResponse>.Failure("Not authorized.");
        return Result<MarketResearchResponse>.Success(MapToResponse(entity));
    }

    private static MarketResearchResponse MapToResponse(MarketResearch m)
    {
        var competitors = (m.Competitors ?? Array.Empty<Competitor>())
            .Select(c => new CompetitorResponse(c.Id, c.Name, c.Website, c.Strengths, c.Weaknesses, c.MarketShareEstimate)).ToList();
        var trends = (m.Trends ?? Array.Empty<MarketTrend>())
            .Select(t => new MarketTrendResponse(t.Id, t.TrendName, t.Description, t.Direction, t.ImpactScore)).ToList();
        return new MarketResearchResponse(
            m.Id, m.Industry, m.TargetMarket, m.GeographicScope,
            m.EstimatedMarketSize, m.MarketGrowthRate,
            m.CompetitorAnalysis, m.TrendAnalysis, m.OpportunityInsights, m.GeneratedReport,
            competitors, trends, m.CreatedAt
        );
    }
}
