using Microsoft.EntityFrameworkCore;
using Startawy.Domain.Entities;
using Startawy.Domain.Interfaces;
using Startawy.Infrastructure.Data;

namespace Startawy.Infrastructure.Repositories;

public class ConsultantRepository : IConsultantRepository
{
    private readonly AppDbContext _db;

    public ConsultantRepository(AppDbContext db) => _db = db;

    public async Task<Consultant?> GetByUserIdAsync(string userId, CancellationToken ct = default)
        => await _db.Consultants.Include(c => c.User).FirstOrDefaultAsync(c => c.UserId == userId, ct);

    public async Task<IReadOnlyList<Consultant>> GetAllAsync(CancellationToken ct = default)
        => await _db.Consultants.Include(c => c.User).ToListAsync(ct);
}

