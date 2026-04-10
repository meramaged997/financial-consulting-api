using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Startawy.Application.Common;
using Startawy.Application.DTOs.Package;
using Startawy.Application.Interfaces;

namespace Startawy.API.Controllers;

[ApiController]
[Route("api/packages")]
[Produces("application/json")]
public class PackagesController : ControllerBase
{
    private readonly IPackageService _service;

    public PackagesController(IPackageService service)
    {
        _service = service;
    }

    // Public: list packages
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var res = await _service.GetAllAsync();
        return Ok(res);
    }

    // Public: get package
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(string id)
    {
        var res = await _service.GetByIdAsync(id);
        if (!res.Success) return NotFound(res);
        return Ok(res);
    }

    // Admin only: create package
    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Create([FromBody] CreatePackageRequest request)
    {
        var res = await _service.CreateAsync(request);
        if (!res.Success) return BadRequest(res);
        return CreatedAtAction(nameof(Get), new { id = res.Data?.PackageId }, res);
    }

    // Admin only: update package
    [HttpPut("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdatePackageRequest request)
    {
        var res = await _service.UpdateAsync(id, request);
        if (!res.Success) return BadRequest(res);
        return Ok(res);
    }

    // Admin only: delete package
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(string id)
    {
        var res = await _service.DeleteAsync(id);
        if (!res.Success) return NotFound(res);
        return Ok(ApiResponse<object?>.Ok(null, "Package deleted."));
    }
}
