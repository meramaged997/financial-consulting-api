using startawy.Core.Entities;
using startawy.Core.Enums;

namespace startawy.Core.Interfaces.Repositories;

public interface IFinancialRepository : IRepository<FinancialStatement>
{
    Task<IReadOnlyList<FinancialStatement>> GetByUserAsync(string userId, StatementType? type = null, CancellationToken ct = default);
    Task<FinancialStatement?>               GetLatestByUserAsync(string userId, CancellationToken ct = default);
}
