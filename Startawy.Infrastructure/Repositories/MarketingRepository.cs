using startawy.Core.Entities;
using startawy.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Startawy.Infrastructure.Data;
using Startawy.Infrastructure.Repositories;

namespace startawy.Infrastructure.Repositories;

public class MarketingRepository : Repository<MarketingCampaign>, IMarketingRepository
{
    public MarketingRepository(AppDbContext db) : base(db) { }

    public async Task<IReadOnlyList<MarketingCampaign>> GetByUserAsync(string userId, CancellationToken ct = default)
        => await _db.MarketingCampaigns.Where(m => m.UserId == userId).OrderByDescending(m => m.CreatedAt).ToListAsync(ct);
}
