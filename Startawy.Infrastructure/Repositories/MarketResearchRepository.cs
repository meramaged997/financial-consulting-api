using startawy.Core.Entities;
using startawy.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Startawy.Infrastructure.Data;
using Startawy.Infrastructure.Repositories;

namespace startawy.Infrastructure.Repositories;

public class MarketResearchRepository : Repository<MarketResearch>, IMarketResearchRepository
{
    public MarketResearchRepository(AppDbContext db) : base(db) { }

    public async Task<MarketResearch?> GetWithDetailsAsync(int id, CancellationToken ct = default)
        => await _db.MarketResearches
            .Include(m => m.Competitors)
            .Include(m => m.Trends)
            .FirstOrDefaultAsync(m => m.Id == id, ct);

    public async Task<IReadOnlyList<MarketResearch>> GetByUserAsync(string userId, CancellationToken ct = default)
        => await _db.MarketResearches
            .Include(m => m.Competitors)
            .Include(m => m.Trends)
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync(ct);
}
