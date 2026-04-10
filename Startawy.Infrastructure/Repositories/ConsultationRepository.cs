using startawy.Core.Entities;
using startawy.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Startawy.Infrastructure.Data;
using Startawy.Infrastructure.Repositories;

namespace startawy.Infrastructure.Repositories;

public class ConsultationRepository : Repository<ConsultationRequest>, IConsultationRepository
{
    public ConsultationRepository(AppDbContext db) : base(db) { }

    public async Task<IReadOnlyList<ConsultationRequest>> GetByUserAsync(string userId, CancellationToken ct = default)
        => await _db.ConsultationRequests.Where(c => c.UserId == userId).OrderByDescending(c => c.RequestedAt).ToListAsync(ct);

    public new async Task<IReadOnlyList<ConsultationRequest>> GetAllAsync(CancellationToken ct = default)
        => await _db.ConsultationRequests.OrderByDescending(c => c.RequestedAt).ToListAsync(ct);
}
