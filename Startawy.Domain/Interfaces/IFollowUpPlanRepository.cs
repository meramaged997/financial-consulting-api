using startawy.Core.Entities;
using startawy.Core.Interfaces.Repositories;

namespace Startawy.Domain.Interfaces;

public interface IFollowUpPlanRepository : IRepository<FollowUpPlan>
{
    Task<IReadOnlyList<FollowUpPlan>> GetByFounderAsync(string founderUserId, CancellationToken ct = default);
    Task<FollowUpPlan?> GetWithStepsAsync(int planId, CancellationToken ct = default);
}

