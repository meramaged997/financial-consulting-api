using startawy.Application.Common.Models;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.DTOs.Responses;

namespace Startawy.Application.Interfaces;

public interface IFeedbackService
{
    Task<Result<FeedbackResponse>> SubmitAsync(string userId, CreateFeedbackRequest request, CancellationToken ct = default);
    Task<Result<IReadOnlyList<FeedbackResponse>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<FeedbackResponse>> ReviewAsync(string adminUserId, int feedbackId, ReviewFeedbackRequest request, CancellationToken ct = default);
}

