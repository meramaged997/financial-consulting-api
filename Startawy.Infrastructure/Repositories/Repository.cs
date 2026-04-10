using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using startawy.Core.Entities;
using startawy.Core.Interfaces.Repositories;
using Startawy.Infrastructure.Data;

namespace Startawy.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _db;

    public Repository(AppDbContext db) => _db = db;

    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _db.Set<T>().FirstOrDefaultAsync(e => e.Id == id, ct);

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default)
        => await _db.Set<T>().ToListAsync(ct);

    public virtual async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await _db.Set<T>().Where(predicate).ToListAsync(ct);

    public virtual async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        await _db.Set<T>().AddAsync(entity, ct);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public virtual async Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        _db.Set<T>().Update(entity);
        await _db.SaveChangesAsync(ct);
    }

    public virtual async Task DeleteAsync(T entity, CancellationToken ct = default)
    {
        _db.Set<T>().Remove(entity);
        await _db.SaveChangesAsync(ct);
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await _db.Set<T>().AnyAsync(predicate, ct);

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await _db.Set<T>().CountAsync(predicate, ct);
}
