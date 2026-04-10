namespace Startawy.Application.DTOs.Responses;

public record ConsultationSessionResponse(
    int Id,
    string FounderUserId,
    string ConsultantUserId,
    DateTime StartAtUtc,
    DateTime EndAtUtc,
    string Status,
    decimal Fee,
    string? PaymentTransactionId,
    string? ConsultantNotes,
    string? ConsultantRecommendations
);

