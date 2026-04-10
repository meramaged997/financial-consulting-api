using startawy.Core.Enums;

namespace Startawy.Application.DTOs.Requests;

public record UpdateConsultationStatusRequest(
    ConsultationStatus Status,
    string?            Notes
);
