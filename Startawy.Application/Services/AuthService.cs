using Startawy.Application.Common;
using Startawy.Application.DTOs.Auth;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.DTOs.Responses;
using Startawy.Application.Interfaces;
using Startawy.Domain.Entities;
using Startawy.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace Startawy.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IPackageRepository _packageRepository;
    private readonly IMemoryCache _cache;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService,
        ISubscriptionRepository subscriptionRepository,
        IPackageRepository packageRepository,
        IMemoryCache cache,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
        _subscriptionRepository = subscriptionRepository;
        _packageRepository = packageRepository;
        _cache = cache;
        _logger = logger;
    }

    private async Task<string> GetUserPackageAsync(string userId, CancellationToken ct = default)
    {
        var sub = await _subscriptionRepository.GetActiveByUserAsync(userId, ct);
        return sub?.Package?.Type ?? "Free";
    }

    public async Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        _logger.LogInformation("Register attempt for {Email}", request.Email);
        bool emailExists = await _userRepository.EmailExistsAsync(request.Email);
        if (emailExists)
        {
            _logger.LogWarning("Register failed: email exists {Email}", request.Email);
            return ApiResponse<AuthResponse>.Fail("This email address is already registered.");
        }

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

        // Business rule: Free plan is automatically assigned on registration.
        var freePackage = await _packageRepository.GetByTypeAsync("Free");
        if (freePackage is null)
        {
            _logger.LogError("Free package missing in system configuration");
            return ApiResponse<AuthResponse>.Fail("System configuration error: Free package is missing. Please contact support.");
        }

        await _subscriptionRepository.DeactivateActiveForUserAsync(created.UserId);
        await _subscriptionRepository.CreateAsync(new Subscription
        {
            SubsId = Guid.NewGuid().ToString(),
            UserId = created.UserId,
            PackageId = freePackage.PackageId,
            Status = "Active",
            TrialType = "Free",
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
            EndDate = null
        });

        var packageType = await GetUserPackageAsync(created.UserId);
        var token = _jwtService.GenerateToken(created, packageType);
        var expiry = _jwtService.GetTokenExpiry();

        _logger.LogInformation("User registered {UserId} with package {Package}", created.UserId, packageType);

        return ApiResponse<AuthResponse>.Ok(
            MapToAuthResponse(created, token, expiry),
            "Registration successful. Welcome to Startawy!");
    }

    public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request)
    {
        _logger.LogInformation("Login attempt for {Email}", request.Email);

        var user = await _userRepository.GetByEmailAsync(request.Email.ToLower().Trim());
        if (user is null)
        {
            _logger.LogWarning("Login failed - user not found {Email}", request.Email);
            return ApiResponse<AuthResponse>.Fail("Invalid email or password.");
        }

        bool passwordValid = _passwordHasher.Verify(request.Password, user.Password);
        if (!passwordValid)
        {
            _logger.LogWarning("Login failed - invalid password for {Email}", request.Email);
            return ApiResponse<AuthResponse>.Fail("Invalid email or password.");
        }

        var packageType = await GetUserPackageAsync(user.UserId);
        var token = _jwtService.GenerateToken(user, packageType);
        var expiry = _jwtService.GetTokenExpiry();

        _logger.LogInformation("Login successful for {UserId} (package {Package})", user.UserId, packageType);

        return ApiResponse<AuthResponse>.Ok(
            MapToAuthResponse(user, token, expiry),
            $"Welcome back, {user.Name}!");
    }

    public async Task<ApiResponse<ForgotPasswordResponse>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken ct = default)
    {
        // Avoid user enumeration: always respond with success.
        _ = ct; // reserved for future repository overloads

        var normalizedEmail = request.Email.ToLower().Trim();
        var user = await _userRepository.GetByEmailAsync(normalizedEmail);

        // Create a short-lived reset token (demo/dev flow).
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(16)); // 32 hex chars
        var expiresAt = DateTime.UtcNow.AddMinutes(15);

        // Store only if user exists (otherwise still return success).
        if (user is not null)
        {
            var cacheKey = $"pwdreset:{normalizedEmail}";
            _cache.Set(cacheKey, token, expiresAt);
        }

        return ApiResponse<ForgotPasswordResponse>.Ok(
            new ForgotPasswordResponse(token, expiresAt),
            "If the email exists, a reset token has been generated."
        );
    }

    public async Task<ApiResponse<object?>> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct = default)
    {
        _ = ct;
        var normalizedEmail = request.Email.ToLower().Trim();
        var user = await _userRepository.GetByEmailAsync(normalizedEmail);

        // Keep response generic to avoid enumeration.
        if (user is null)
            return ApiResponse<object?>.Fail("Invalid reset token or email.");

        var cacheKey = $"pwdreset:{normalizedEmail}";
        if (!_cache.TryGetValue<string>(cacheKey, out var storedToken) ||
            !string.Equals(storedToken, request.ResetToken, StringComparison.OrdinalIgnoreCase))
        {
            return ApiResponse<object?>.Fail("Invalid reset token or email.");
        }

        user.Password = _passwordHasher.Hash(request.NewPassword);
        await _userRepository.UpdateAsync(user);
        _cache.Remove(cacheKey);

        return ApiResponse<object?>.Ok(null, "Password reset successful.");
    }

    public async Task<ApiResponse<AuthResponse>> ExternalLoginAsync(string provider, ExternalLoginRequest request, CancellationToken ct = default)
    {
        // Stub endpoint so frontend can integrate UI flow.
        // Proper implementation requires validating provider token (Google/Facebook) and mapping to a user.
        _ = request;
        _ = ct;
        return await Task.FromResult(ApiResponse<AuthResponse>.Fail($"External login via '{provider}' is not configured on the server yet."));
    }

    public async Task<ApiResponse<object?>> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        if (string.IsNullOrEmpty(userId))
            return ApiResponse<object?>.Fail("Unauthorized.");

        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            return ApiResponse<object?>.Fail("User not found.");

        var currentValid = _passwordHasher.Verify(request.CurrentPassword, user.Password);
        if (!currentValid)
            return ApiResponse<object?>.Fail("Current password is incorrect.");

        if (string.IsNullOrWhiteSpace(request.NewPassword))
            return ApiResponse<object?>.Fail("New password is required.");

        if (!string.IsNullOrEmpty(request.ConfirmPassword) && request.NewPassword != request.ConfirmPassword)
            return ApiResponse<object?>.Fail("New password and confirmation do not match.");

        user.Password = _passwordHasher.Hash(request.NewPassword);
        await _userRepository.UpdateAsync(user);

        return ApiResponse<object?>.Ok(null, "Password changed successfully.");
    }

    private static AuthResponse MapToAuthResponse(User user, string token, DateTime expiry)
        => new()
        {
            UserId = user.UserId,
            FullName = user.Name,
            Email = user.Email,
            PhoneNumber = user.Phone ?? string.Empty,
            Role = user.Type,
            RedirectTo = user.Type switch
            {
                "StartupFounder" => "/founder/dashboard",
                "FinancialConsultant" => "/consultant/dashboard",
                "Administrator" => "/admin/dashboard",
                _ => "/"
            },
            Token = token,
            TokenExpiry = expiry
        };
}