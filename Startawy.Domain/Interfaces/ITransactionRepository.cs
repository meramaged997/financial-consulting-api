using Startawy.Domain.Entities;

namespace Startawy.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<Transaction> AddAsync(Transaction transaction, CancellationToken ct = default);
    Task<Transaction?> GetByIdAsync(string transactionId, CancellationToken ct = default);
    Task<Transaction?> GetByIdempotencyKeyAsync(string userId, string idempotencyKey, CancellationToken ct = default);
    Task UpdateAsync(Transaction transaction, CancellationToken ct = default);
    Task<IReadOnlyList<Transaction>> GetByUserAsync(string userId, CancellationToken ct = default);
}

