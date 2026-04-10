using Microsoft.EntityFrameworkCore;
using startawy.Core.Entities;
using Startawy.Domain.Interfaces;
using Startawy.Infrastructure.Data;
using Startawy.Infrastructure.Repositories;

namespace Startawy.Infrastructure.Repositories;

public class ConsultantAvailabilityRepository : Repository<ConsultantAvailabilitySlot>, IConsultantAvailabilityRepository
{
    public ConsultantAvailabilityRepository(AppDbContext db) : base(db) { }

    public async Task<IReadOnlyList<ConsultantAvailabilitySlot>> GetOpenSlotsAsync(string consultantUserId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default)
        => await _db.ConsultantAvailabilitySlots
            .Where(s => s.ConsultantUserId == consultantUserId &&
                        !s.IsBooked &&
                        s.StartAtUtc >= fromUtc &&
                        s.EndAtUtc <= toUtc)
            .OrderBy(s => s.StartAtUtc)
            .ToListAsync(ct);
}

