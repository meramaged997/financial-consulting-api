using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Startawy.Application.Common;
using Startawy.Application.DTOs.Requests;
using Startawy.Application.DTOs.Responses;
using Startawy.Infrastructure.Data;

namespace Startawy.API.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : BaseController
{
    private readonly AppDbContext _db;

    public ProfileController(AppDbContext db) => _db = db;

    [HttpGet("me")]
    public async Task<IActionResult> GetMe(CancellationToken ct = default)
    {
        var user = await _db.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == CurrentUserId, ct);

        if (user is null)
            return NotFound(ApiResponse<object>.Fail("User not found."));

        var res = new UserProfileResponse
        {
            UserId = user.UserId,
            FullName = user.Name,
            Email = user.Email,
            PhoneNumber = user.Phone ?? string.Empty,
            Role = user.Type
        };

        return Ok(ApiResponse<UserProfileResponse>.Ok(res));
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateProfileRequest request, CancellationToken ct = default)
    {
        var user = await _db.Users
            .Include(u => u.StartupFounder)
            .FirstOrDefaultAsync(u => u.UserId == CurrentUserId, ct);

        if (user is null)
            return NotFound(ApiResponse<object>.Fail("User not found."));

        var first = request.FirstName?.Trim();
        var last = request.LastName?.Trim();
        var fullName = string.Join(' ', new[] { first, last }.Where(s => !string.IsNullOrWhiteSpace(s)));
        if (!string.IsNullOrWhiteSpace(fullName))
            user.Name = fullName;

        if (user.StartupFounder is not null)
        {
            if (!string.IsNullOrWhiteSpace(request.CompanyName))
                user.StartupFounder.BusinessName = request.CompanyName.Trim();
            if (!string.IsNullOrWhiteSpace(request.Industry))
                user.StartupFounder.BusinessSector = request.Industry.Trim();
        }

        await _db.SaveChangesAsync(ct);

        var res = new UserProfileResponse
        {
            UserId = user.UserId,
            FullName = user.Name,
            Email = user.Email,
            PhoneNumber = user.Phone ?? string.Empty,
            Role = user.Type
        };

        return Ok(ApiResponse<UserProfileResponse>.Ok(res, "Profile updated."));
    }

    // Alias used by frontend: PUT /api/profile/update
    [HttpPut("update")]
    public Task<IActionResult> UpdateAlias([FromBody] UpdateProfileRequest request, CancellationToken ct = default)
        => UpdateMe(request, ct);
}

