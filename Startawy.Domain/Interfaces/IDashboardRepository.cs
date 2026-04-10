using startawy.Core.Entities;

namespace startawy.Core.Interfaces.Repositories;

public interface IDashboardRepository : IRepository<DashboardSnapshot>
{
    Task<IReadOnlyList<DashboardSnapshot>> GetRecentByUserAsync(string userId, int count = 6, CancellationToken ct = default);
}
