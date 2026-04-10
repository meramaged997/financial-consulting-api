using startawy.Application.Common.Models;
using startawy.Core.Entities;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.DTOs.Responses;
using Startawy.Application.Interfaces;
using Startawy.Domain.Interfaces;

namespace Startawy.Application.Services;

public class FollowUpPlanService : IFollowUpPlanService
{
    private readonly IFollowUpPlanRepository _repo;
    private readonly ISubscriptionRepository _subscriptionRepo;

    public FollowUpPlanService(IFollowUpPlanRepository repo, ISubscriptionRepository subscriptionRepo)
    {
        _repo = repo;
        _subscriptionRepo = subscriptionRepo;
    }

    public async Task<Result<FollowUpPlanResponse>> CreateAsync(string consultantUserId, CreateFollowUpPlanRequest request, CancellationToken ct = default)
    {
        var founderSub = await _subscriptionRepo.GetActiveByUserAsync(request.FounderUserId, ct);
        var pkg = founderSub?.Package?.Type ?? "Free";
        if (!pkg.Equals("Premium", StringComparison.OrdinalIgnoreCase))
            return Result<FollowUpPlanResponse>.Failure("Follow-up plans are available for Premium subscribers only.");

        if (request.TimelineEndUtc <= request.TimelineStartUtc)
            return Result<FollowUpPlanResponse>.Failure("Timeline end must be after timeline start.");

        var plan = new FollowUpPlan
        {
            FounderUserId = request.FounderUserId,
            ConsultantUserId = consultantUserId,
            Goal = request.Goal.Trim(),
            TimelineStartUtc = request.TimelineStartUtc,
            TimelineEndUtc = request.TimelineEndUtc,
            CreatedBy = consultantUserId
        };

        foreach (var s in request.Steps)
        {
            plan.Steps.Add(new FollowUpStep
            {
                Title = s.Title.Trim(),
                Description = s.Description?.Trim(),
                DueAtUtc = s.DueAtUtc,
                Status = "Pending"
            });
        }

        var created = await _repo.AddAsync(plan, ct);
        var loaded = await _repo.GetWithStepsAsync(created.Id, ct) ?? created;
        return Result<FollowUpPlanResponse>.Success(Map(loaded));
    }

    public async Task<Result<IReadOnlyList<FollowUpPlanResponse>>> GetForFounderAsync(string founderUserId, CancellationToken ct = default)
    {
        var list = await _repo.GetByFounderAsync(founderUserId, ct);
        return Result<IReadOnlyList<FollowUpPlanResponse>>.Success(list.Select(Map).ToList());
    }

    private static FollowUpPlanResponse Map(FollowUpPlan p)
        => new(
            p.Id,
            p.FounderUserId,
            p.ConsultantUserId,
            p.Goal,
            p.TimelineStartUtc,
            p.TimelineEndUtc,
            (p.Steps ?? Array.Empty<FollowUpStep>())
                .OrderBy(s => s.DueAtUtc)
                .Select(s => new FollowUpStepResponse(s.Id, s.Title, s.Description, s.DueAtUtc, s.Status))
                .ToList(),
            p.CreatedAt
        );
}

