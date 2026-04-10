using startawy.Core.Enums;

namespace startawy.Application.DTOs.Responses;

public record ConsultationResponse(
    int                 Id,
    string              Subject,
    string              Description,
    ConsultationType    Type,
    ConsultationStatus  Status,
    string              PreAnalysis,
    string?             ExpertNotes,
    DateTime            RequestedAt,
    DateTime?           ScheduledAt,
    DateTime?           CompletedAt
);
