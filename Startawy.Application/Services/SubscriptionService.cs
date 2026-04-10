using startawy.Application.Common.Models;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.DTOs.Responses;
using Startawy.Application.Interfaces;
using Startawy.Domain.Entities;
using Startawy.Domain.Interfaces;

namespace Startawy.Application.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subsRepo;
    private readonly IPackageRepository _packageRepo;
    private readonly ITransactionRepository _txRepo;

    public SubscriptionService(
        ISubscriptionRepository subsRepo,
        IPackageRepository packageRepo,
        ITransactionRepository txRepo)
    {
        _subsRepo = subsRepo;
        _packageRepo = packageRepo;
        _txRepo = txRepo;
    }

    public async Task<Result<SubscriptionResponse>> GetMySubscriptionAsync(string userId, CancellationToken ct = default)
    {
        var active = await _subsRepo.GetActiveByUserAsync(userId, ct);
        if (active is null)
            return Result<SubscriptionResponse>.Failure("No active subscription found.");

        return Result<SubscriptionResponse>.Success(new SubscriptionResponse(
            active.SubsId,
            active.Package?.Type ?? "Free",
            active.Status,
            active.StartDate,
            active.EndDate
        ));
    }

    public async Task<Result<PaymentResponse>> UpgradeAsync(string userId, UpgradePackageRequest request, CancellationToken ct = default)
    {
        var pkg = await _packageRepo.GetByIdAsync(request.PackageId);
        if (pkg is null) return Result<PaymentResponse>.Failure("Package not found.");

        var pkgType = pkg.Type ?? "Free";
        if (pkgType.Equals("Free", StringComparison.OrdinalIgnoreCase))
            return Result<PaymentResponse>.Failure("Free plan does not require payment.");

        var price = pkg.Price ?? 0m;
        if (price <= 0m)
            return Result<PaymentResponse>.Failure("Invalid package price configuration.");

        // Deactivate any active subscription and create a new one.
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

        // Business rule: create transaction record after successful payment.
        var tx = await _txRepo.AddAsync(new Transaction
        {
            TransId = Guid.NewGuid().ToString(),
            UserId = userId,
            SubsId = sub.SubsId,
            Amount = price,
            PaymentMethod = request.PaymentMethod,
            Type = "SubscriptionPayment",
            TransDate = DateOnly.FromDateTime(DateTime.UtcNow)
        }, ct);

        return Result<PaymentResponse>.Success(new PaymentResponse(
            tx.TransId,
            tx.Amount,
            tx.Type,
            tx.PaymentMethod ?? string.Empty,
            tx.TransDate,
            sub.SubsId,
            pkgType
        ));
    }
}

