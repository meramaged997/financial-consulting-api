namespace Startawy.Application.DTOs.Responses;

public record SubscriptionResponse(
    string SubsId,
    string PackageType,
    string Status,
    DateOnly StartDate,
    DateOnly? EndDate
);

