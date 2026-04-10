using Microsoft.EntityFrameworkCore;
using startawy.Core.Entities;
using Startawy.Domain.Interfaces;
using Startawy.Infrastructure.Data;
using startawy.Infrastructure.Repositories;

namespace Startawy.Infrastructure.Repositories;

public class ConsultationSessionRepository : Repository<ConsultationSession>, IConsultationSessionRepository
{
    public ConsultationSessionRepository(AppDbContext db) : base(db) { }

    public async Task<IReadOnlyList<ConsultationSession>> GetByFounderAsync(string founderUserId, CancellationToken ct = default)
        => await _db.ConsultationSessions
            .Where(s => s.FounderUserId == founderUserId)
            .OrderByDescending(s => s.StartAtUtc)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<ConsultationSession>> GetByConsultantAsync(string consultantUserId, CancellationToken ct = default)
        => await _db.ConsultationSessions
            .Where(s => s.ConsultantUserId == consultantUserId)
            .OrderByDescending(s => s.StartAtUtc)
            .ToListAsync(ct);
}

