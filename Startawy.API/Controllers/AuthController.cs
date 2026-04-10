using Microsoft.AspNetCore.Mvc;
using Startawy.Application.Common;
using Startawy.Application.DTOs.Auth;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Startawy.API.Controllers;

[ApiController]
[Route("auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, IJwtService jwtService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _jwtService = jwtService;
        _logger = logger;
    }

    /// <summary>
    /// Register a new user (Startup Founder, Consultant, or Admin).
    /// </summary>
    /// <param name="request">Registration payload</param>
    /// <returns>JWT token + user info on success</returns>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        _logger.LogInformation("Register attempt for {Email}", request.Email);

        var result = await _authService.RegisterAsync(request);

        if (!result.Success)
        {
            _logger.LogWarning("Register failed for {Email}: {Message}", request.Email, result.Message);
            // Email conflict → 409
            if (result.Message.Contains("already registered"))
                return Conflict(ApiResponse<object>.Fail(result.Message, result.Errors));

            return BadRequest(ApiResponse<object>.Fail(result.Message, result.Errors));
        }

        _logger.LogInformation("Register successful for {Email}", request.Email);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>
    /// Login with email and password.
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>JWT token + user info on success</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Login attempt for {Email}", request.Email);

        var result = await _authService.LoginAsync(request);

        if (!result.Success)
        {
            _logger.LogWarning("Login failed for {Email}: {Message}", request.Email, result.Message);
            return Unauthorized(ApiResponse<object>.Fail(result.Message, result.Errors));
        }

        _logger.LogInformation("Login successful for {Email}", request.Email);

        return Ok(result);
    }

    /// <summary>
    /// Generate a password reset token (dev/demo).
    /// </summary>
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken ct = default)
    {
        var result = await _authService.ForgotPasswordAsync(request, ct);
        return Ok(result);
    }

    /// <summary>
    /// Reset password using a previously generated token, or change password for authenticated users.
    /// Frontend posts { CurrentPassword, NewPassword, ConfirmPassword } for authenticated change.
    /// </summary>
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] JsonElement payload, CancellationToken ct = default)
    {
        // If user is authenticated and front-end sent CurrentPassword => perform change password.
        // Guard against non-object payloads to avoid JsonElement invalid operations that can cause 500s.
        if (User?.Identity?.IsAuthenticated == true && payload.ValueKind == JsonValueKind.Object && (payload.TryGetProperty("CurrentPassword", out _) || payload.TryGetProperty("currentPassword", out _)))
        {
            string? current = null, @new = null, confirm = null;
            if (payload.TryGetProperty("CurrentPassword", out var cp)) current = cp.GetString();
            if (payload.TryGetProperty("currentPassword", out cp)) current ??= cp.GetString();
            if (payload.TryGetProperty("NewPassword", out var np)) @new = np.GetString();
            if (payload.TryGetProperty("newPassword", out np)) @new ??= np.GetString();
            if (payload.TryGetProperty("ConfirmPassword", out var cf)) confirm = cf.GetString();
            if (payload.TryGetProperty("confirmPassword", out cf)) confirm ??= cf.GetString();

            if (string.IsNullOrWhiteSpace(current) || string.IsNullOrWhiteSpace(@new))
                return BadRequest(ApiResponse<object>.Fail("CurrentPassword and NewPassword are required."));

            var userId = User.FindFirst("userId")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(ApiResponse<object>.Fail("Unauthorized."));

            var changeReq = new ChangePasswordRequest(current, @new, confirm);
            var changeResult = await _authService.ChangePasswordAsync(userId, changeReq);
            return changeResult.Success ? Ok(changeResult) : BadRequest(changeResult);
        }

        // Otherwise treat as token-based reset (email + reset token + new password)
        try
        {
            var req = JsonSerializer.Deserialize<ResetPasswordRequest>(payload.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (req is null)
                return BadRequest(ApiResponse<object>.Fail("Invalid request payload."));

            var result = await _authService.ResetPasswordAsync(req, ct);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        catch (JsonException)
        {
            return BadRequest(ApiResponse<object>.Fail("Invalid JSON payload."));
        }
    }

    /// <summary>
    /// External login (Google/Facebook) - stub until OAuth configured.
    /// </summary>
    [HttpPost("external/{provider}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ExternalLogin([FromRoute] string provider, [FromBody] ExternalLoginRequest request, CancellationToken ct = default)
    {
        var result = await _authService.ExternalLoginAsync(provider, request, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
