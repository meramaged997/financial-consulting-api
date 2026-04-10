using startawy.Core.Entities;
using startawy.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Startawy.Infrastructure.Data;
using Startawy.Infrastructure.Repositories;

namespace startawy.Infrastructure.Repositories;

public class BudgetRepository : Repository<BudgetAnalysis>, IBudgetRepository
{
    public BudgetRepository(AppDbContext db) : base(db) { }

    public async Task<BudgetAnalysis?> GetWithLineItemsAsync(int id, CancellationToken ct = default)
        => await _db.BudgetAnalyses.Include(b => b.LineItems).FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<IReadOnlyList<BudgetAnalysis>> GetByUserAsync(string userId, CancellationToken ct = default)
        => await _db.BudgetAnalyses.Include(b => b.LineItems)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.AnalysisDate)
            .ToListAsync(ct);
}
