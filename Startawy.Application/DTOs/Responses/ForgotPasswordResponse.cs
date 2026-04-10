namespace Startawy.Application.DTOs.Responses;

public record ForgotPasswordResponse(
    // Returned for dev/demo flows. In production you'd email it instead.
    string ResetToken,
    DateTime ExpiresAtUtc
);

