using Startawy.Application.DTOs.Requests;
using startawy.Application.Common.Models;
using startawy.Application.DTOs.Responses;

namespace Startawy.Application.Interfaces;

public interface IConsultationService
{
    Task<Result<ConsultationResponse>> CreateAsync(string userId, CreateConsultationRequest request, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ConsultationResponse>>> GetMineAsync(string userId, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ConsultationResponse>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<ConsultationResponse>> UpdateStatusAsync(int consultationId, UpdateConsultationStatusRequest request, CancellationToken ct = default);
}
