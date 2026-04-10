using startawy.Application.Common.Models;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.DTOs.Responses;

namespace Startawy.Application.Interfaces;

public interface ISessionsService
{
    Task<Result<AvailabilitySlotResponse>> CreateAvailabilityAsync(string consultantUserId, CreateAvailabilitySlotRequest request, CancellationToken ct = default);
    Task<Result<IReadOnlyList<AvailabilitySlotResponse>>> GetAvailabilityAsync(string consultantUserId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default);

    Task<Result<ConsultationSessionResponse>> BookAsync(string founderUserId, BookSessionRequest request, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ConsultationSessionResponse>>> GetMyFounderSessionsAsync(string founderUserId, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ConsultationSessionResponse>>> GetMyConsultantSessionsAsync(string consultantUserId, CancellationToken ct = default);
    Task<Result<ConsultationSessionResponse>> CompleteAsync(string consultantUserId, int sessionId, CompleteSessionRequest request, CancellationToken ct = default);
}

