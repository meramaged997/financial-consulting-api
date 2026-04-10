namespace Startawy.Application.DTOs.Requests;

public record ResetPasswordRequest(
    string Email,
    string ResetToken,
    string NewPassword
);

