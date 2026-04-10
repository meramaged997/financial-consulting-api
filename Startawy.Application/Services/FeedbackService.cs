using startawy.Application.Common.Models;
using startawy.Core.Entities;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.DTOs.Responses;
using Startawy.Application.Interfaces;
using Startawy.Domain.Interfaces;

namespace Startawy.Application.Services;

public class FeedbackService : IFeedbackService
{
    private readonly IFeedbackRepository _repo;

    public FeedbackService(IFeedbackRepository repo) => _repo = repo;

    public async Task<Result<FeedbackResponse>> SubmitAsync(string userId, CreateFeedbackRequest request, CancellationToken ct = default)
    {
        var entity = await _repo.AddAsync(new Feedback
        {
            UserId = userId,
            Message = request.Message.Trim(),
            Category = "Uncategorized",
            IsReviewed = false,
            ReviewedAtUtc = null,
            ReviewedByAdminId = null,
            CreatedBy = userId
        }, ct);

        return Result<FeedbackResponse>.Success(Map(entity));
    }

    public async Task<Result<IReadOnlyList<FeedbackResponse>>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await _repo.GetAllOrderedAsync(ct);
        return Result<IReadOnlyList<FeedbackResponse>>.Success(list.Select(Map).ToList());
    }

    public async Task<Result<FeedbackResponse>> ReviewAsync(string adminUserId, int feedbackId, ReviewFeedbackRequest request, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(feedbackId, ct);
        if (entity is null) return Result<FeedbackResponse>.Failure("Feedback not found.");

        entity.Category = request.Category;
        entity.IsReviewed = request.MarkReviewed;
        entity.ReviewedByAdminId = adminUserId;
        entity.ReviewedAtUtc = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = adminUserId;

        await _repo.UpdateAsync(entity, ct);
        return Result<FeedbackResponse>.Success(Map(entity));
    }

    private static FeedbackResponse Map(Feedback f)
        => new(f.Id, f.UserId, f.Message, f.Category, f.IsReviewed, f.ReviewedByAdminId, f.ReviewedAtUtc, f.CreatedAt);
}

