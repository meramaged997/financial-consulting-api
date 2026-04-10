using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Startawy.Application.Common;
using Startawy.Application.Interfaces;
using Startawy.Application.DTOs.Requests;
using Startawy.Domain.Entities;
using Startawy.Infrastructure.Data;

namespace Startawy.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Administrator")]
[Produces("application/json")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher _passwordHasher;

    public AdminController(AppDbContext db, IPasswordHasher passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    // Covers: /admin/dashboard and /admin/analytics
    [HttpGet("analytics")]
    public async Task<IActionResult> Analytics(CancellationToken ct = default)
    {
        var totalUsers = await _db.Users.CountAsync(ct);
        var founders = await _db.Users.CountAsync(u => u.Type == "StartupFounder", ct);
        var consultants = await _db.Users.CountAsync(u => u.Type == "FinancialConsultant", ct);
        var feedback = await _db.Feedbacks.CountAsync(ct);
        var transactions = await _db.Transactions.CountAsync(ct);

        return Ok(ApiResponse<object>.Ok(new
        {
            totalUsers,
            founders,
            consultants,
            feedback,
            transactions
        }));
    }

    [HttpGet("founders")]
    public async Task<IActionResult> GetFounders(CancellationToken ct = default)
    {
        var items = await _db.Users.AsNoTracking()
            .Where(u => u.Type == "StartupFounder")
            .Select(u => new { userId = u.UserId, fullName = u.Name, email = u.Email, phoneNumber = u.Phone })
            .ToListAsync(ct);

        return Ok(ApiResponse<object>.Ok(items));
    }

    [HttpGet("consultants")]
    public async Task<IActionResult> GetConsultants(CancellationToken ct = default)
    {
        var items = await _db.Consultants.AsNoTracking()
            .Include(c => c.User)
            .Select(c => new
            {
                userId = c.UserId,
                fullName = c.User.Name,
                email = c.User.Email,
                phoneNumber = c.User.Phone,
                specialization = c.Specialization,
                yearsOfExp = c.YearsOfExp,
                sessionRate = c.SessionRate
            })
            .ToListAsync(ct);

        return Ok(ApiResponse<object>.Ok(items));
    }

    // Covers: /admin/add-consultant
    [HttpPost("consultants")]
    public async Task<IActionResult> AddConsultant([FromBody] CreateConsultantRequest request, CancellationToken ct = default)
    {
        var normalizedEmail = request.Email.ToLower().Trim();
        var exists = await _db.Users.AnyAsync(u => u.Email == normalizedEmail, ct);
        if (exists)
            return Conflict(ApiResponse<object>.Fail("This email address is already registered."));

        var user = new User
        {
            UserId = Guid.NewGuid().ToString(),
            Name = request.FullName.Trim(),
            Email = normalizedEmail,
            Password = _passwordHasher.Hash(request.Password),
            Type = "FinancialConsultant"
        };

        var consultant = new Consultant
        {
            UserId = user.UserId,
            YearsOfExp = request.YearsOfExp,
            Specialization = request.Specialization?.Trim(),
            SessionRate = request.SessionRate
        };

        _db.Users.Add(user);
        _db.Consultants.Add(consultant);
        await _db.SaveChangesAsync(ct);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<object>.Ok(new { userId = user.UserId }, "Consultant created."));
    }
}

