using System.Linq.Expressions;
using startawy.Core.Entities;

namespace startawy.Core.Interfaces.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?>                  GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<T>>    GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<T>>    FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<T>                   AddAsync(T entity, CancellationToken ct = default);
    Task                      UpdateAsync(T entity, CancellationToken ct = default);
    Task                      DeleteAsync(T entity, CancellationToken ct = default);
    Task<bool>                ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<int>                 CountAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
}
