namespace Startawy.Application.DTOs.Responses;

public record FollowUpPlanResponse(
    int Id,
    string FounderUserId,
    string ConsultantUserId,
    string Goal,
    DateTime TimelineStartUtc,
    DateTime TimelineEndUtc,
    List<FollowUpStepResponse> Steps,
    DateTime CreatedAt
);

public record FollowUpStepResponse(
    int Id,
    string Title,
    string? Description,
    DateTime DueAtUtc,
    string Status
);

