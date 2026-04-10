using Microsoft.EntityFrameworkCore;
using startawy.Core.Entities;
using Startawy.Domain.Interfaces;
using Startawy.Infrastructure.Data;
using startawy.Infrastructure.Repositories;

namespace Startawy.Infrastructure.Repositories;

public class FollowUpPlanRepository : Repository<FollowUpPlan>, IFollowUpPlanRepository
{
    public FollowUpPlanRepository(AppDbContext db) : base(db) { }

    public async Task<IReadOnlyList<FollowUpPlan>> GetByFounderAsync(string founderUserId, CancellationToken ct = default)
        => await _db.FollowUpPlans
            .Include(p => p.Steps.OrderBy(s => s.DueAtUtc))
            .Where(p => p.FounderUserId == founderUserId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(ct);

    public async Task<FollowUpPlan?> GetWithStepsAsync(int planId, CancellationToken ct = default)
        => await _db.FollowUpPlans
            .Include(p => p.Steps.OrderBy(s => s.DueAtUtc))
            .FirstOrDefaultAsync(p => p.Id == planId, ct);
}

