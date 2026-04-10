namespace Startawy.Application.DTOs.Requests;

public record CreateConsultantRequest(
    string FullName,
    string Email,
    string Password,
    int? YearsOfExp,
    string? Specialization,
    decimal SessionRate
);

