namespace Startawy.Application.DTOs.Requests;

public record UpdateProfileRequest(
    string? FirstName,
    string? LastName,
    string? CompanyName,
    string? Industry
);
