using startawy.Core.Entities;
using startawy.Core.Interfaces.Repositories;

namespace Startawy.Domain.Interfaces;

public interface IConsultantAvailabilityRepository : IRepository<ConsultantAvailabilitySlot>
{
    Task<IReadOnlyList<ConsultantAvailabilitySlot>> GetOpenSlotsAsync(string consultantUserId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default);
}

