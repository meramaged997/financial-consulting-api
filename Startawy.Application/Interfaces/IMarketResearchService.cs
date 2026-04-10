using Startawy.Application.DTOs.Requests;
using startawy.Application.Common.Models;
using startawy.Application.DTOs.Responses;

namespace Startawy.Application.Interfaces;

public interface IMarketResearchService
{
    Task<Result<MarketResearchResponse>> CreateAsync(string userId, CreateMarketResearchRequest request, CancellationToken ct = default);
    Task<Result<IReadOnlyList<MarketResearchResponse>>> GetAllAsync(string userId, CancellationToken ct = default);
    Task<Result<MarketResearchResponse>> GetByIdAsync(string userId, int id, CancellationToken ct = default);
}
