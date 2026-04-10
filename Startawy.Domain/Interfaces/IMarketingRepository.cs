using startawy.Core.Entities;

namespace startawy.Core.Interfaces.Repositories;

public interface IMarketingRepository : IRepository<MarketingCampaign>
{
    Task<IReadOnlyList<MarketingCampaign>> GetByUserAsync(string userId, CancellationToken ct = default);
}
