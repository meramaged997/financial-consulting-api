using Startawy.Domain.Entities;

namespace Startawy.Domain.Interfaces;

public interface IConsultantRepository
{
    Task<Consultant?> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Task<IReadOnlyList<Consultant>> GetAllAsync(CancellationToken ct = default);
}

