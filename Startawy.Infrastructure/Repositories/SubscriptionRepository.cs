using Microsoft.EntityFrameworkCore;
using Startawy.Domain.Entities;
using Startawy.Domain.Interfaces;
using Startawy.Infrastructure.Data;

namespace Startawy.Infrastructure.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly AppDbContext _db;

    public SubscriptionRepository(AppDbContext db) => _db = db;

    public async Task<Subscription?> GetActiveByUserAsync(string userId, CancellationToken ct = default)
        => await _db.Subscriptions
            .Include(s => s.Package)
            .Where(s => s.UserId == userId && s.Status == "Active")
            .OrderByDescending(s => s.StartDate)
            .FirstOrDefaultAsync(ct);

    public async Task<Subscription> CreateAsync(Subscription subscription, CancellationToken ct = default)
    {
        await _db.Subscriptions.AddAsync(subscription, ct);
        await _db.SaveChangesAsync(ct);
        return subscription;
    }

    public async Task DeactivateActiveForUserAsync(string userId, CancellationToken ct = default)
    {
        var active = await _db.Subscriptions.Where(s => s.UserId == userId && s.Status == "Active").ToListAsync(ct);
        if (active.Count == 0) return;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        foreach (var s in active)
        {
            s.Status = "Inactive";
            s.EndDate ??= today;
        }

        await _db.SaveChangesAsync(ct);
    }
}

