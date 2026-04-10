using startawy.Core.Entities;
using startawy.Core.Interfaces.Repositories;

namespace Startawy.Domain.Interfaces;

public interface IFeedbackRepository : IRepository<Feedback>
{
    Task<IReadOnlyList<Feedback>> GetAllOrderedAsync(CancellationToken ct = default);
}

