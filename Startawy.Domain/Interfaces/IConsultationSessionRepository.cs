using startawy.Core.Entities;
using startawy.Core.Interfaces.Repositories;

namespace Startawy.Domain.Interfaces;

public interface IConsultationSessionRepository : IRepository<ConsultationSession>
{
    Task<IReadOnlyList<ConsultationSession>> GetByFounderAsync(string founderUserId, CancellationToken ct = default);
    Task<IReadOnlyList<ConsultationSession>> GetByConsultantAsync(string consultantUserId, CancellationToken ct = default);
}

