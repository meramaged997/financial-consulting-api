using startawy.Core.Entities;

namespace startawy.Core.Interfaces.Repositories;

public interface IBudgetRepository : IRepository<BudgetAnalysis>
{
    Task<BudgetAnalysis?>          GetWithLineItemsAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<BudgetAnalysis>> GetByUserAsync(string userId, CancellationToken ct = default);
}
