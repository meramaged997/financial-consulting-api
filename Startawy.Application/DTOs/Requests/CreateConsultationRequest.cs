using startawy.Core.Enums;

namespace Startawy.Application.DTOs.Requests;

public record CreateConsultationRequest(
    string           Subject,
    string           Description,
    ConsultationType Type,
    DateTime?        PreferredDate
);
