using startawy.Core.Entities;

namespace startawy.Core.Interfaces.Repositories;

public interface ICashFlowRepository : IRepository<CashFlowForecast>
{
    Task<CashFlowForecast?>               GetWithMonthlyDataAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<CashFlowForecast>> GetByUserAsync(string userId, CancellationToken ct = default);
    Task<CashFlowForecast?>               GetLatestByUserAsync(string userId, CancellationToken ct = default);
}
