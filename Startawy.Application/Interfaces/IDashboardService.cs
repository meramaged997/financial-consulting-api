using startawy.Application.Common.Models;
using startawy.Application.DTOs.Responses;

namespace Startawy.Application.Interfaces;

public interface IDashboardService
{
    Task<Result<DashboardResponse>> GetAsync(string userId, CancellationToken ct = default);
}
