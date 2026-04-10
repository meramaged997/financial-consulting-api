using startawy.Application.Common.Models;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.DTOs.Responses;

namespace Startawy.Application.Interfaces;

public interface IFollowUpPlanService
{
    Task<Result<FollowUpPlanResponse>> CreateAsync(string consultantUserId, CreateFollowUpPlanRequest request, CancellationToken ct = default);
    Task<Result<IReadOnlyList<FollowUpPlanResponse>>> GetForFounderAsync(string founderUserId, CancellationToken ct = default);
}

