using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Startawy.Application.Common;
using Startawy.Infrastructure.Data;

namespace Startawy.API.Controllers;

[ApiController]
[Route("api/consultant")]
[Authorize(Roles = "FinancialConsultant")]
[Produces("application/json")]
public class ConsultantPortalController : BaseController
{
    private readonly AppDbContext _db;

    public ConsultantPortalController(AppDbContext db) => _db = db;

    // Covers: /consultant/dashboard
    [HttpGet("dashboard")]
    public IActionResult Dashboard()
        => Ok(ApiResponse<object>.Ok(new { message = "Consultant dashboard (demo)." }));

    // Covers: /consultant/clients
    [HttpGet("clients")]
    public async Task<IActionResult> Clients(CancellationToken ct = default)
    {
        var founderIds = await _db.Sessions.AsNoTracking()
            .Where(s => s.ConsultantId == CurrentUserId)
            .Select(s => s.FounderId)
            .Distinct()
            .ToListAsync(ct);

        var founders = await _db.Users.AsNoTracking()
            .Where(u => founderIds.Contains(u.UserId))
            .Select(u => new { userId = u.UserId, fullName = u.Name, email = u.Email, phoneNumber = u.Phone })
            .ToListAsync(ct);

        return Ok(ApiResponse<object>.Ok(founders));
    }

    // Covers: /consultant/client/:id
    [HttpGet("client/{userId}")]
    public async Task<IActionResult> ClientDetails([FromRoute] string userId, CancellationToken ct = default)
    {
        var founder = await _db.Users.AsNoTracking()
            .Where(u => u.UserId == userId)
            .Select(u => new { userId = u.UserId, fullName = u.Name, email = u.Email, phoneNumber = u.Phone })
            .FirstOrDefaultAsync(ct);

        return founder is null
            ? NotFound(ApiResponse<object>.Fail("Client not found."))
            : Ok(ApiResponse<object>.Ok(founder));
    }

    // Covers: /consultant/client/:id/add-review (stub)
    [HttpPost("client/{userId}/add-review")]
    public IActionResult AddReview([FromRoute] string userId)
        => Ok(ApiResponse<object>.Ok(new { userId }, "Review saved (demo)."));

    // Covers: /consultant/client/:id/schedule-meeting (stub)
    [HttpPost("client/{userId}/schedule-meeting")]
    public IActionResult ScheduleMeeting([FromRoute] string userId)
        => Ok(ApiResponse<object>.Ok(new { userId }, "Meeting scheduled (demo)."));

    // Covers: /consultant/client/:id/send-message (stub)
    [HttpPost("client/{userId}/send-message")]
    public IActionResult SendMessage([FromRoute] string userId)
        => Ok(ApiResponse<object>.Ok(new { userId }, "Message sent (demo)."));

    // Covers: /consultant/client/:id/generate-report (stub)
    [HttpGet("client/{userId}/generate-report")]
    public IActionResult GenerateClientReport([FromRoute] string userId)
        => Ok(ApiResponse<object>.Ok(new { userId, reportId = Guid.NewGuid().ToString() }, "Report generated (demo)."));

    // Covers: /consultant/earnings (simple calc)
    [HttpGet("earnings")]
    public async Task<IActionResult> Earnings(CancellationToken ct = default)
    {
        var sessionsCount = await _db.Sessions.AsNoTracking()
            .CountAsync(s => s.ConsultantId == CurrentUserId, ct);

        var rate = await _db.Consultants.AsNoTracking()
            .Where(c => c.UserId == CurrentUserId)
            .Select(c => c.SessionRate)
            .FirstOrDefaultAsync(ct);

        return Ok(ApiResponse<object>.Ok(new
        {
            sessionsCount,
            sessionRate = rate,
            estimatedTotal = sessionsCount * rate
        }));
    }

    // Covers: /consultant/recommendations (stub)
    [HttpGet("recommendations")]
    public IActionResult Recommendations()
        => Ok(ApiResponse<object>.Ok(Array.Empty<object>(), "No recommendations yet (demo)."));

    // Covers: /consultant/startup-details (stub)
    [HttpGet("startup-details")]
    public IActionResult StartupDetails()
        => Ok(ApiResponse<object>.Ok(new { message = "Startup details endpoint (demo)." }));

    // Covers: /consultant/follow-up-plans (data lives in /api/follow-up-plans)
    [HttpGet("follow-up-plans")]
    public IActionResult FollowUpPlansHint()
        => Ok(ApiResponse<object>.Ok(new { use = "/api/follow-up-plans" }));
}

