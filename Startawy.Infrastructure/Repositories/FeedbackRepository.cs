using Microsoft.EntityFrameworkCore;
using startawy.Core.Entities;
using Startawy.Domain.Interfaces;
using Startawy.Infrastructure.Data;
using startawy.Infrastructure.Repositories;

namespace Startawy.Infrastructure.Repositories;

public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
{
    public FeedbackRepository(AppDbContext db) : base(db) { }

    public async Task<IReadOnlyList<Feedback>> GetAllOrderedAsync(CancellationToken ct = default)
        => await _db.Feedbacks
            .OrderByDescending(f => f.CreatedAt)
            .ToListAsync(ct);
}

