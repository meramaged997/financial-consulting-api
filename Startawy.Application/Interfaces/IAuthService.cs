using Startawy.Application.Common;
using Startawy.Application.DTOs.Auth;

namespace Startawy.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request);
    Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request);
}
