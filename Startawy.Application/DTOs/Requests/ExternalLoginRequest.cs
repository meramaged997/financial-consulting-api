namespace Startawy.Application.DTOs.Requests;

public record ExternalLoginRequest(
    // Frontend sends either OAuth access_token or ID token depending on provider.
    string Token
);

