using startawy.Core.Entities;
using startawy.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Startawy.Infrastructure.Data;
using Startawy.Infrastructure.Repositories;

namespace startawy.Infrastructure.Repositories;

public class DashboardRepository : Repository<DashboardSnapshot>, IDashboardRepository
{
    public DashboardRepository(AppDbContext db) : base(db) { }

    public async Task<IReadOnlyList<DashboardSnapshot>> GetRecentByUserAsync(string userId, int count = 6, CancellationToken ct = default)
        => await _db.DashboardSnapshots.Where(d => d.UserId == userId)
            .OrderByDescending(d => d.SnapshotDate).Take(count).ToListAsync(ct);
}
