using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Startawy.Application.Common;
using Startawy.Infrastructure.Data;

namespace Startawy.API.Controllers;

[ApiController]
[Route("api/consultants")]
[Produces("application/json")]
public class ConsultantsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ConsultantsController(AppDbContext db) => _db = db;

    // Public list used by: /book-consultant and /consultant/:id
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
    {
        var consultants = await _db.Consultants
            .AsNoTracking()
            .Include(c => c.User)
            .Select(c => new
            {
                userId = c.UserId,
                fullName = c.User.Name,
                email = c.User.Email,
                specialization = c.Specialization,
                yearsOfExp = c.YearsOfExp,
                sessionRate = c.SessionRate
            })
            .ToListAsync(ct);

        return Ok(ApiResponse<object>.Ok(consultants));
    }

    [HttpGet("{userId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById([FromRoute] string userId, CancellationToken ct = default)
    {
        var consultant = await _db.Consultants
            .AsNoTracking()
            .Include(c => c.User)
            .Where(c => c.UserId == userId)
            .Select(c => new
            {
                userId = c.UserId,
                fullName = c.User.Name,
                email = c.User.Email,
                phoneNumber = c.User.Phone,
                specialization = c.Specialization,
                yearsOfExp = c.YearsOfExp,
                certificate = c.Certificate,
                availability = c.Availability,
                sessionRate = c.SessionRate
            })
            .FirstOrDefaultAsync(ct);

        if (consultant is null)
            return NotFound(ApiResponse<object>.Fail("Consultant not found."));

        return Ok(ApiResponse<object>.Ok(consultant));
    }
}

