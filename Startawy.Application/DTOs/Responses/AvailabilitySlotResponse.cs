namespace Startawy.Application.DTOs.Responses;

public record AvailabilitySlotResponse(
    int Id,
    string ConsultantUserId,
    DateTime StartAtUtc,
    DateTime EndAtUtc,
    bool IsBooked
);

