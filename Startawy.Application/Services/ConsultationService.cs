using startawy.Core.Entities;
using startawy.Core.Enums;
using startawy.Core.Interfaces.Repositories;
using startawy.Application.Common.Models;
using Startawy.Application.DTOs.Requests;
using startawy.Application.DTOs.Responses;
using Startawy.Application.Interfaces;

namespace Startawy.Application.Services;

public class ConsultationService : IConsultationService
{
    private readonly IConsultationRepository _consultationRepo;

    public ConsultationService(IConsultationRepository consultationRepo) => _consultationRepo = consultationRepo;

    public async Task<Result<ConsultationResponse>> CreateAsync(string userId, CreateConsultationRequest request, CancellationToken ct = default)
    {
        var entity = new ConsultationRequest
        {
            UserId = userId,
            Subject = request.Subject,
            Description = request.Description,
            Type = request.Type,
            Status = ConsultationStatus.Pending,
            PreAnalysis = string.Empty,
            RequestedAt = DateTime.UtcNow,
            ScheduledAt = request.PreferredDate,
            CreatedBy = userId
        };
        var added = await _consultationRepo.AddAsync(entity, ct);
        return Result<ConsultationResponse>.Success(MapToResponse(added));
    }

    public async Task<Result<IReadOnlyList<ConsultationResponse>>> GetMineAsync(string userId, CancellationToken ct = default)
    {
        var list = await _consultationRepo.GetByUserAsync(userId, ct);
        return Result<IReadOnlyList<ConsultationResponse>>.Success(list.Select(MapToResponse).ToList());
    }

    public async Task<Result<IReadOnlyList<ConsultationResponse>>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await _consultationRepo.GetAllAsync(ct);
        return Result<IReadOnlyList<ConsultationResponse>>.Success(list.Select(MapToResponse).ToList());
    }

    public async Task<Result<ConsultationResponse>> UpdateStatusAsync(int consultationId, UpdateConsultationStatusRequest request, CancellationToken ct = default)
    {
        var entity = await _consultationRepo.GetByIdAsync(consultationId, ct);
        if (entity is null) return Result<ConsultationResponse>.Failure("Consultation not found.");
        entity.Status = request.Status;
        entity.ExpertNotes = request.Notes;
        if (request.Status == ConsultationStatus.Completed)
            entity.CompletedAt = DateTime.UtcNow;
        await _consultationRepo.UpdateAsync(entity, ct);
        return Result<ConsultationResponse>.Success(MapToResponse(entity));
    }

    private static ConsultationResponse MapToResponse(ConsultationRequest c)
    {
        return new ConsultationResponse(
            c.Id,
            c.Subject,
            c.Description,
            c.Type,
            c.Status,
            c.PreAnalysis,
            c.ExpertNotes,
            c.RequestedAt,
            c.ScheduledAt,
            c.CompletedAt
        );
    }
}
