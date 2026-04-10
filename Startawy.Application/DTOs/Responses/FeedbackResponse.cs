namespace Startawy.Application.DTOs.Responses;

public record FeedbackResponse(
    int Id,
    string UserId,
    string Message,
    string Category,
    bool IsReviewed,
    string? ReviewedByAdminId,
    DateTime? ReviewedAtUtc,
    DateTime CreatedAt
);

