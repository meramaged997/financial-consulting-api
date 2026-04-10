using startawy.Core.Entities;
using startawy.Core.Enums;
using startawy.Core.Interfaces.Repositories;
using startawy.Application.Common.Models;
using Startawy.Application.DTOs.Requests;
using startawy.Application.DTOs.Responses;
using Startawy.Application.Interfaces;

namespace Startawy.Application.Services;

public class MarketingService : IMarketingService
{
    private readonly IMarketingRepository _marketingRepo;

    public MarketingService(IMarketingRepository marketingRepo) => _marketingRepo = marketingRepo;

    public async Task<Result<MarketingCampaignResponse>> CreateAsync(string userId, CreateMarketingCampaignRequest request, CancellationToken ct = default)
    {
        var entity = new MarketingCampaign
        {
            UserId = userId,
            CampaignName = request.CampaignName,
            BusinessType = request.BusinessType,
            TargetAudience = request.TargetAudience,
            Budget = request.Budget,
            Strategy = string.Empty,
            ChannelRecommendations = string.Empty,
            ContentCalendar = string.Empty,
            Status = CampaignStatus.Draft,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            CreatedBy = userId
        };
        var added = await _marketingRepo.AddAsync(entity, ct);
        return Result<MarketingCampaignResponse>.Success(MapToResponse(added));
    }

    public async Task<Result<IReadOnlyList<MarketingCampaignResponse>>> GetAllAsync(string userId, CancellationToken ct = default)
    {
        var list = await _marketingRepo.GetByUserAsync(userId, ct);
        return Result<IReadOnlyList<MarketingCampaignResponse>>.Success(list.Select(MapToResponse).ToList());
    }

    public async Task<Result<MarketingCampaignResponse>> UpdateStatusAsync(string userId, int id, CampaignStatus status, CancellationToken ct = default)
    {
        var entity = await _marketingRepo.GetByIdAsync(id, ct);
        if (entity is null) return Result<MarketingCampaignResponse>.Failure("Campaign not found.");
        if (entity.UserId != userId) return Result<MarketingCampaignResponse>.Failure("Not authorized.");
        entity.Status = status;
        await _marketingRepo.UpdateAsync(entity, ct);
        return Result<MarketingCampaignResponse>.Success(MapToResponse(entity));
    }

    private static MarketingCampaignResponse MapToResponse(MarketingCampaign m)
    {
        return new MarketingCampaignResponse(
            m.Id, m.CampaignName, m.BusinessType, m.TargetAudience, m.Budget,
            m.Strategy, m.ChannelRecommendations, m.ContentCalendar,
            m.Status, m.StartDate, m.EndDate, m.CreatedAt
        );
    }
}
