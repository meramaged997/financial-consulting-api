using Startawy.Application.Common;
using Startawy.Application.DTOs.Auth;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.DTOs.Responses;

namespace Startawy.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request);
    Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request);
    Task<ApiResponse<ForgotPasswordResponse>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct = default);
    Task<ApiResponse<object?>> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct = default);
    Task<ApiResponse<object?>> ChangePasswordAsync(string userId, ChangePasswordRequest request);
    Task<ApiResponse<AuthResponse>> ExternalLoginAsync(string provider, ExternalLoginRequest request, CancellationToken ct = default);
}
