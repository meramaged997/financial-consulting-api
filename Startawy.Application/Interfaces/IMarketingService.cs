using Startawy.Application.DTOs.Requests;
using startawy.Application.Common.Models;
using startawy.Application.DTOs.Responses;
using startawy.Core.Enums;

namespace Startawy.Application.Interfaces;

public interface IMarketingService
{
    Task<Result<MarketingCampaignResponse>> CreateAsync(string userId, CreateMarketingCampaignRequest request, CancellationToken ct = default);
    Task<Result<IReadOnlyList<MarketingCampaignResponse>>> GetAllAsync(string userId, CancellationToken ct = default);
    Task<Result<MarketingCampaignResponse>> UpdateStatusAsync(string userId, int id, CampaignStatus status, CancellationToken ct = default);
}
