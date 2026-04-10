using startawy.Application.Common.Models;
using startawy.Core.Entities;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.DTOs.Responses;
using Startawy.Application.Interfaces;
using Startawy.Domain.Entities;
using Startawy.Domain.Interfaces;

namespace Startawy.Application.Services;

public class SessionsService : ISessionsService
{
    private readonly IConsultantAvailabilityRepository _availabilityRepo;
    private readonly IConsultationSessionRepository _sessionRepo;
    private readonly IConsultantRepository _consultantRepo;
    private readonly ITransactionRepository _txRepo;

    public SessionsService(
        IConsultantAvailabilityRepository availabilityRepo,
        IConsultationSessionRepository sessionRepo,
        IConsultantRepository consultantRepo,
        ITransactionRepository txRepo)
    {
        _availabilityRepo = availabilityRepo;
        _sessionRepo = sessionRepo;
        _consultantRepo = consultantRepo;
        _txRepo = txRepo;
    }

    public async Task<Result<AvailabilitySlotResponse>> CreateAvailabilityAsync(string consultantUserId, CreateAvailabilitySlotRequest request, CancellationToken ct = default)
    {
        if (request.EndAtUtc <= request.StartAtUtc)
            return Result<AvailabilitySlotResponse>.Failure("End time must be after start time.");

        if (request.StartAtUtc <= DateTime.UtcNow)
            return Result<AvailabilitySlotResponse>.Failure("Availability slot must be in the future.");

        // Prevent overlapping slots for the same consultant.
        var overlaps = await _availabilityRepo.ExistsAsync(s =>
            s.ConsultantUserId == consultantUserId &&
            !s.IsBooked &&
            s.StartAtUtc < request.EndAtUtc &&
            s.EndAtUtc > request.StartAtUtc, ct);
        if (overlaps)
            return Result<AvailabilitySlotResponse>.Failure("This slot overlaps an existing availability slot.");

        var slot = await _availabilityRepo.AddAsync(new ConsultantAvailabilitySlot
        {
            ConsultantUserId = consultantUserId,
            StartAtUtc = request.StartAtUtc,
            EndAtUtc = request.EndAtUtc,
            IsBooked = false
        }, ct);

        return Result<AvailabilitySlotResponse>.Success(Map(slot));
    }

    public async Task<Result<IReadOnlyList<AvailabilitySlotResponse>>> GetAvailabilityAsync(string consultantUserId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default)
    {
        if (toUtc <= fromUtc) return Result<IReadOnlyList<AvailabilitySlotResponse>>.Failure("Invalid date range.");
        var list = await _availabilityRepo.GetOpenSlotsAsync(consultantUserId, fromUtc, toUtc, ct);
        return Result<IReadOnlyList<AvailabilitySlotResponse>>.Success(list.Select(Map).ToList());
    }

    public async Task<Result<ConsultationSessionResponse>> BookAsync(string founderUserId, BookSessionRequest request, CancellationToken ct = default)
    {
        var slot = await _availabilityRepo.GetByIdAsync(request.AvailabilitySlotId, ct);
        if (slot is null) return Result<ConsultationSessionResponse>.Failure("Availability slot not found.");
        if (slot.IsBooked) return Result<ConsultationSessionResponse>.Failure("This slot is already booked.");
        if (slot.StartAtUtc <= DateTime.UtcNow) return Result<ConsultationSessionResponse>.Failure("This slot is no longer available.");

        var consultant = await _consultantRepo.GetByUserIdAsync(slot.ConsultantUserId, ct);
        if (consultant is null) return Result<ConsultationSessionResponse>.Failure("Consultant not found.");

        // Business rule: ensure consultant availability before booking (no overlapping booked sessions).
        var overlappingBooked = await _sessionRepo.ExistsAsync(s =>
            s.ConsultantUserId == slot.ConsultantUserId &&
            s.Status == "Booked" &&
            s.StartAtUtc < slot.EndAtUtc &&
            s.EndAtUtc > slot.StartAtUtc, ct);
        if (overlappingBooked)
            return Result<ConsultationSessionResponse>.Failure("Consultant is no longer available in this time slot.");

        var fee = consultant.SessionRate;

        // Business rule: session is paid separately from subscription.
        var tx = await _txRepo.AddAsync(new Transaction
        {
            TransId = Guid.NewGuid().ToString(),
            UserId = founderUserId,
            SubsId = null,
            Amount = fee,
            PaymentMethod = request.PaymentMethod,
            Type = "SessionPayment",
            TransDate = DateOnly.FromDateTime(DateTime.UtcNow)
        }, ct);

        var session = await _sessionRepo.AddAsync(new ConsultationSession
        {
            FounderUserId = founderUserId,
            ConsultantUserId = slot.ConsultantUserId,
            StartAtUtc = slot.StartAtUtc,
            EndAtUtc = slot.EndAtUtc,
            Status = "Booked",
            Fee = fee,
            PaymentTransactionId = tx.TransId,
            CreatedBy = founderUserId
        }, ct);

        slot.IsBooked = true;
        slot.ConsultationSessionId = session.Id;
        await _availabilityRepo.UpdateAsync(slot, ct);

        return Result<ConsultationSessionResponse>.Success(Map(session));
    }

    public async Task<Result<IReadOnlyList<ConsultationSessionResponse>>> GetMyFounderSessionsAsync(string founderUserId, CancellationToken ct = default)
    {
        var list = await _sessionRepo.GetByFounderAsync(founderUserId, ct);
        return Result<IReadOnlyList<ConsultationSessionResponse>>.Success(list.Select(Map).ToList());
    }

    public async Task<Result<IReadOnlyList<ConsultationSessionResponse>>> GetMyConsultantSessionsAsync(string consultantUserId, CancellationToken ct = default)
    {
        var list = await _sessionRepo.GetByConsultantAsync(consultantUserId, ct);
        return Result<IReadOnlyList<ConsultationSessionResponse>>.Success(list.Select(Map).ToList());
    }

    public async Task<Result<ConsultationSessionResponse>> CompleteAsync(string consultantUserId, int sessionId, CompleteSessionRequest request, CancellationToken ct = default)
    {
        var session = await _sessionRepo.GetByIdAsync(sessionId, ct);
        if (session is null) return Result<ConsultationSessionResponse>.Failure("Session not found.");
        if (session.ConsultantUserId != consultantUserId) return Result<ConsultationSessionResponse>.Failure("Not authorized.");
        if (session.Status == "Completed") return Result<ConsultationSessionResponse>.Failure("Session already completed.");

        session.Status = "Completed";
        session.ConsultantNotes = request.Notes;
        session.ConsultantRecommendations = request.Recommendations;
        session.UpdatedAt = DateTime.UtcNow;
        session.UpdatedBy = consultantUserId;
        await _sessionRepo.UpdateAsync(session, ct);

        return Result<ConsultationSessionResponse>.Success(Map(session));
    }

    private static AvailabilitySlotResponse Map(ConsultantAvailabilitySlot s)
        => new(s.Id, s.ConsultantUserId, s.StartAtUtc, s.EndAtUtc, s.IsBooked);

    private static ConsultationSessionResponse Map(ConsultationSession s)
        => new(s.Id, s.FounderUserId, s.ConsultantUserId, s.StartAtUtc, s.EndAtUtc, s.Status, s.Fee, s.PaymentTransactionId, s.ConsultantNotes, s.ConsultantRecommendations);
}

