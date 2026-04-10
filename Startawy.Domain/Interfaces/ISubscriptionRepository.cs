using Startawy.Domain.Entities;

namespace Startawy.Domain.Interfaces;

public interface ISubscriptionRepository
{
    Task<Subscription?> GetActiveByUserAsync(string userId, CancellationToken ct = default);

    Task<Subscription> CreateAsync(Subscription subscription, CancellationToken ct = default);
    Task DeactivateActiveForUserAsync(string userId, CancellationToken ct = default);
}

