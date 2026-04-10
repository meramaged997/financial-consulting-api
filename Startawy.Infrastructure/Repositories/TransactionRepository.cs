using Microsoft.EntityFrameworkCore;
using Startawy.Domain.Entities;
using Startawy.Domain.Interfaces;
using Startawy.Infrastructure.Data;

namespace Startawy.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _db;

    public TransactionRepository(AppDbContext db) => _db = db;

    public async Task<Transaction> AddAsync(Transaction transaction, CancellationToken ct = default)
    {
        await _db.Transactions.AddAsync(transaction, ct);
        await _db.SaveChangesAsync(ct);
        return transaction;
    }

    public async Task<Transaction?> GetByIdAsync(string transactionId, CancellationToken ct = default)
        => await _db.Transactions.FirstOrDefaultAsync(t => t.TransId == transactionId, ct);

    public async Task<Transaction?> GetByIdempotencyKeyAsync(string userId, string idempotencyKey, CancellationToken ct = default)
        => await _db.Transactions.FirstOrDefaultAsync(t => t.UserId == userId && t.IdempotencyKey == idempotencyKey, ct);

    public async Task UpdateAsync(Transaction transaction, CancellationToken ct = default)
    {
        _db.Transactions.Update(transaction);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<Transaction>> GetByUserAsync(string userId, CancellationToken ct = default)
        => await _db.Transactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.TransDate)
            .ToListAsync(ct);
}

