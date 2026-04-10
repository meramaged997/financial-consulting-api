using System.Security.Claims;
using startawy.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Startawy.Application.Common;

namespace Startawy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException();

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess && result.Value is not null)
            return Ok(ApiResponse<T>.Ok(result.Value));

        if (!result.IsSuccess && result.ValidationErrors is not null)
        {
            var errors = result.ValidationErrors
                .SelectMany(kvp => kvp.Value.Select(v => $"{kvp.Key}: {v}"))
                .ToList();

            return BadRequest(ApiResponse<T>.Fail("Validation failed.", errors));
        }

        if (!result.IsSuccess)
            return BadRequest(ApiResponse<T>.Fail(result.Error ?? "Request failed."));

        return NotFound(ApiResponse<T>.Fail("The requested resource was not found."));
    }

    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
            return Ok(ApiResponse<object?>.Ok(null, "Operation successful."));

        return BadRequest(ApiResponse<object?>.Fail(result.Error ?? "Request failed."));
    }
}
