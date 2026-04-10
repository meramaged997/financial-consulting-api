using startawy.Core.Enums;

namespace startawy.Application.DTOs.Responses;

public record AuthResponse(
    string      Token,
    string      UserId,
    string      Email,
    string      FullName,
    PackageType Package,
    DateTime    ExpiresAt
);
