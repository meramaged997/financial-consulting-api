using Startawy.Application.Common;
using Startawy.Application.DTOs.Auth;
using Startawy.Application.Interfaces;
using Startawy.Domain.Entities;
using Startawy.Domain.Interfaces;

namespace Startawy.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        bool emailExists = await _userRepository.EmailExistsAsync(request.Email);
        if (emailExists)
            return ApiResponse<AuthResponse>.Fail("This email address is already registered.");

        var user = new User
        {
            UserId = Guid.NewGuid().ToString(),
            Name = request.FullName.Trim(),
            Email = request.Email.ToLower().Trim(),
            Password = _passwordHasher.Hash(request.Password),
            Phone = request.PhoneNumber?.Trim(),
            Type = request.Role.ToString()
        };

        var created = await _userRepository.CreateAsync(user);
        var token = _jwtService.GenerateToken(created);
        var expiry = _jwtService.GetTokenExpiry();

        return ApiResponse<AuthResponse>.Ok(
            MapToAuthResponse(created, token, expiry),
            "Registration successful. Welcome to Startawy!");
    }

    public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email.ToLower().Trim());
        if (user is null)
            return ApiResponse<AuthResponse>.Fail("Invalid email or password.");

        bool passwordValid = _passwordHasher.Verify(request.Password, user.Password);
        if (!passwordValid)
            return ApiResponse<AuthResponse>.Fail("Invalid email or password.");

        var token = _jwtService.GenerateToken(user);
        var expiry = _jwtService.GetTokenExpiry();

        return ApiResponse<AuthResponse>.Ok(
            MapToAuthResponse(user, token, expiry),
            $"Welcome back, {user.Name}!");
    }

    private static AuthResponse MapToAuthResponse(User user, string token, DateTime expiry)
        => new()
        {
            UserId = 0,
            FullName = user.Name,
            Email = user.Email,
            PhoneNumber = user.Phone ?? string.Empty,
            Role = user.Type,
            Token = token,
            TokenExpiry = expiry
        };
}