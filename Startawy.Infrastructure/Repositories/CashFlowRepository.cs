using startawy.Core.Entities;
using startawy.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Startawy.Infrastructure.Data;
using Startawy.Infrastructure.Repositories;

namespace startawy.Infrastructure.Repositories;

public class CashFlowRepository : Repository<CashFlowForecast>, ICashFlowRepository
{
    public CashFlowRepository(AppDbContext db) : base(db) { }

    public async Task<CashFlowForecast?> GetWithMonthlyDataAsync(int id, CancellationToken ct = default)
        => await _db.CashFlowForecasts.Include(c => c.MonthlyForecasts).FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<IReadOnlyList<CashFlowForecast>> GetByUserAsync(string userId, CancellationToken ct = default)
        => await _db.CashFlowForecasts.Include(c => c.MonthlyForecasts)
            .Where(c => c.UserId == userId).OrderByDescending(c => c.CreatedAt).ToListAsync(ct);

    public async Task<CashFlowForecast?> GetLatestByUserAsync(string userId, CancellationToken ct = default)
        => await _db.CashFlowForecasts.Include(c => c.MonthlyForecasts)
            .Where(c => c.UserId == userId).OrderByDescending(c => c.CreatedAt).FirstOrDefaultAsync(ct);
}
