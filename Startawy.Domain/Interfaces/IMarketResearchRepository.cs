using startawy.Core.Entities;

namespace startawy.Core.Interfaces.Repositories;

public interface IMarketResearchRepository : IRepository<MarketResearch>
{
    Task<MarketResearch?>               GetWithDetailsAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<MarketResearch>> GetByUserAsync(string userId, CancellationToken ct = default);
}
