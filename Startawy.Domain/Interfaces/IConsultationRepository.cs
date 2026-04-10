using startawy.Core.Entities;

namespace startawy.Core.Interfaces.Repositories;

public interface IConsultationRepository : IRepository<ConsultationRequest>
{
    Task<IReadOnlyList<ConsultationRequest>> GetByUserAsync(string userId, CancellationToken ct = default);
    new Task<IReadOnlyList<ConsultationRequest>> GetAllAsync(CancellationToken ct = default);
}
