using startawy.Application.Common.Models;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.DTOs.Responses;
using Startawy.Application.Interfaces;
using Startawy.Domain.Entities;
using Startawy.Domain.Interfaces;

namespace Startawy.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly ITransactionRepository _txRepo;
    private readonly IPackageRepository _packageRepo;
    private readonly ISubscriptionRepository _subsRepo;
    private readonly IConsultantAvailabilityRepository _availabilityRepo;
    private readonly IConsultationSessionRepository _sessionRepo;
    private readonly IConsultantRepository _consultantRepo;

    public PaymentService(
        ITransactionRepository txRepo,
        IPackageRepository packageRepo,
        ISubscriptionRepository subsRepo,
        IConsultantAvailabilityRepository availabilityRepo,
        IConsultationSessionRepository sessionRepo,
        IConsultantRepository consultantRepo)
    {
        _txRepo = txRepo;
        _packageRepo = packageRepo;
        _subsRepo = subsRepo;
        _availabilityRepo = availabilityRepo;
        _sessionRepo = sessionRepo;
        _consultantRepo = consultantRepo;
    }

    public async Task<Result<PaymentIntentResponse>> CreateIntentAsync(
        string userId,
        CreatePaymentIntentRequest request,
        string? idempotencyKey,
        CancellationToken ct = default)
    {
        if (!string.IsNullOrWhiteSpace(idempotencyKey))
        {
            var existing = await _txRepo.GetByIdempotencyKeyAsync(userId, idempotencyKey, ct);
            if (existing is not null)
            {
                return Result<PaymentIntentResponse>.Success(new PaymentIntentResponse(
                    existing.TransId,
                    existing.Status,
                    existing.Amount,
                    "EGP",
                    existing.Type
                ));
            }
        }

        if (request.Purpose == "SubscriptionUpgrade")
        {
            if (string.IsNullOrWhiteSpace(request.PackageId))
                return Result<PaymentIntentResponse>.Failure("PackageId is required for SubscriptionUpgrade.");

            var pkg = await _packageRepo.GetByIdAsync(request.PackageId);
            if (pkg is null) return Result<PaymentIntentResponse>.Failure("Package not found.");
            if (pkg.Type.Equals("Free", StringComparison.OrdinalIgnoreCase))
                return Result<PaymentIntentResponse>.Failure("Free plan does not require payment.");

            var amount = pkg.Price ?? 0m;
            if (amount <= 0m) return Result<PaymentIntentResponse>.Failure("Invalid package price configuration.");

            var tx = await _txRepo.AddAsync(new Transaction
            {
                TransId = Guid.NewGuid().ToString(),
                UserId = userId,
                SubsId = null,
                Amount = amount,
                PaymentMethod = request.PaymentMethod,
                Type = "SubscriptionUpgrade",
                Status = "Pending",
                IdempotencyKey = string.IsNullOrWhiteSpace(idempotencyKey) ? null : idempotencyKey,
                ReferenceType = "Package",
                ReferenceId = pkg.PackageId,
                TransDate = DateOnly.FromDateTime(DateTime.UtcNow)
            }, ct);

            return Result<PaymentIntentResponse>.Success(new PaymentIntentResponse(tx.TransId, tx.Status, tx.Amount, "EGP", tx.Type));
        }

        if (request.Purpose == "SessionBooking")
        {
            if (!request.AvailabilitySlotId.HasValue)
                return Result<PaymentIntentResponse>.Failure("AvailabilitySlotId is required for SessionBooking.");

            var slot = await _availabilityRepo.GetByIdAsync(request.AvailabilitySlotId.Value, ct);
            if (slot is null) return Result<PaymentIntentResponse>.Failure("Availability slot not found.");
            if (slot.IsBooked) return Result<PaymentIntentResponse>.Failure("This slot is already booked.");
            if (slot.StartAtUtc <= DateTime.UtcNow) return Result<PaymentIntentResponse>.Failure("This slot is no longer available.");

            var consultant = await _consultantRepo.GetByUserIdAsync(slot.ConsultantUserId, ct);
            if (consultant is null) return Result<PaymentIntentResponse>.Failure("Consultant not found.");

            var fee = consultant.SessionRate;
            if (fee < 0) fee = 0;

            var tx = await _txRepo.AddAsync(new Transaction
            {
                TransId = Guid.NewGuid().ToString(),
                UserId = userId,
                SubsId = null,
                Amount = fee,
                PaymentMethod = request.PaymentMethod,
                Type = "SessionBooking",
                Status = "Pending",
                IdempotencyKey = string.IsNullOrWhiteSpace(idempotencyKey) ? null : idempotencyKey,
                ReferenceType = "AvailabilitySlot",
                ReferenceId = slot.Id.ToString(),
                TransDate = DateOnly.FromDateTime(DateTime.UtcNow)
            }, ct);

            return Result<PaymentIntentResponse>.Success(new PaymentIntentResponse(tx.TransId, tx.Status, tx.Amount, "EGP", tx.Type));
        }

        return Result<PaymentIntentResponse>.Failure("Invalid payment purpose.");
    }

    public async Task<Result<object>> ConfirmAsync(string userId, string transactionId, ConfirmPaymentRequest request, CancellationToken ct = default)
    {
        var tx = await _txRepo.GetByIdAsync(transactionId, ct);
        if (tx is null) return Result<object>.Failure("Transaction not found.");
        if (tx.UserId != userId) return Result<object>.Failure("Not authorized.");
        if (tx.Status != "Pending") return Result<object>.Failure("Transaction is not pending.");

        tx.Status = request.Succeeded ? "Succeeded" : "Failed";
        tx.ExternalReference = request.ExternalReference;
        tx.TransDate = DateOnly.FromDateTime(DateTime.UtcNow);
        await _txRepo.UpdateAsync(tx, ct);

        if (!request.Succeeded)
            return Result<object>.Success(new { transactionId = tx.TransId, status = tx.Status });

        // Apply business effect based on purpose/type.
        if (tx.Type == "SubscriptionUpgrade" && tx.ReferenceType == "Package" && !string.IsNullOrWhiteSpace(tx.ReferenceId))
        {
            var pkg = await _packageRepo.GetByIdAsync(tx.ReferenceId);
            if (pkg is null) return Result<object>.Failure("Package not found for transaction.");

            await _subsRepo.DeactivateActiveForUserAsync(userId, ct);

            var start = DateOnly.FromDateTime(DateTime.UtcNow);
            var end = (pkg.Duration ?? 30) > 0 ? start.AddDays(pkg.Duration!.Value) : (DateOnly?)null;

            var sub = await _subsRepo.CreateAsync(new Subscription
            {
                SubsId = Guid.NewGuid().ToString(),
                UserId = userId,
                PackageId = pkg.PackageId,
                Status = "Active",
                TrialType = null,
                StartDate = start,
                EndDate = end
            }, ct);

            tx.SubsId = sub.SubsId;
            tx.Type = "SubscriptionPayment";
            await _txRepo.UpdateAsync(tx, ct);

            return Result<object>.Success(new
            {
                transactionId = tx.TransId,
                status = tx.Status,
                subscriptionId = sub.SubsId,
                packageType = pkg.Type
            });
        }

        if (tx.Type == "SessionBooking" && tx.ReferenceType == "AvailabilitySlot" && int.TryParse(tx.ReferenceId, out var slotId))
        {
            var slot = await _availabilityRepo.GetByIdAsync(slotId, ct);
            if (slot is null) return Result<object>.Failure("Availability slot not found for transaction.");
            if (slot.IsBooked) return Result<object>.Failure("Slot already booked.");

            var consultant = await _consultantRepo.GetByUserIdAsync(slot.ConsultantUserId, ct);
            if (consultant is null) return Result<object>.Failure("Consultant not found for transaction.");

            var overlappingBooked = await _sessionRepo.ExistsAsync(s =>
                s.ConsultantUserId == slot.ConsultantUserId &&
                s.Status == "Booked" &&
                s.StartAtUtc < slot.EndAtUtc &&
                s.EndAtUtc > slot.StartAtUtc, ct);
            if (overlappingBooked)
                return Result<object>.Failure("Consultant is no longer available in this time slot.");

            var session = await _sessionRepo.AddAsync(new startawy.Core.Entities.ConsultationSession
            {
                FounderUserId = userId,
                ConsultantUserId = slot.ConsultantUserId,
                StartAtUtc = slot.StartAtUtc,
                EndAtUtc = slot.EndAtUtc,
                Status = "Booked",
                Fee = consultant.SessionRate,
                PaymentTransactionId = tx.TransId,
                CreatedBy = userId
            }, ct);

            slot.IsBooked = true;
            slot.ConsultationSessionId = session.Id;
            await _availabilityRepo.UpdateAsync(slot, ct);

            tx.Type = "SessionPayment";
            await _txRepo.UpdateAsync(tx, ct);

            return Result<object>.Success(new { transactionId = tx.TransId, status = tx.Status, sessionId = session.Id });
        }

        return Result<object>.Success(new { transactionId = tx.TransId, status = tx.Status });
    }
}

