using startawy.Core.Entities;
using startawy.Core.Enums;
using startawy.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Startawy.Infrastructure.Data;
using Startawy.Infrastructure.Repositories;

namespace startawy.Infrastructure.Repositories;

public class FinancialRepository : Repository<FinancialStatement>, IFinancialRepository
{
    public FinancialRepository(AppDbContext db) : base(db) { }

    public async Task<IReadOnlyList<FinancialStatement>> GetByUserAsync(string userId, StatementType? type = null, CancellationToken ct = default)
    {
        var q = _db.FinancialStatements.Where(f => f.UserId == userId);
        if (type.HasValue) q = q.Where(f => f.Type == type.Value);
        return await q.OrderByDescending(f => f.CreatedAt).ToListAsync(ct);
    }

    public async Task<FinancialStatement?> GetLatestByUserAsync(string userId, CancellationToken ct = default)
        => await _db.FinancialStatements.Where(f => f.UserId == userId).OrderByDescending(f => f.CreatedAt).FirstOrDefaultAsync(ct);
}
