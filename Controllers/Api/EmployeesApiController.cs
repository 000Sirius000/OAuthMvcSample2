using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OAuthMvcSample.Data;

namespace OAuthMvcSample.Controllers.Api;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class EmployeesApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmployeesApiController(AppDbContext context)
    {
        _context = context;
    }

    // v1: стара версія (наприклад, тільки базові поля)
    [HttpGet]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetV1()
    {
        var data = await _context.Employees
            .Select(e => new
            {
                e.Id,
                e.FullName,
                e.Email
            })
            .ToListAsync();

        return Ok(data);
    }

    // v2: нова версія (додаємо department/position)
    [HttpGet]
    [MapToApiVersion("2.0")]
    public async Task<IActionResult> GetV2()
    {
        var data = await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .Select(e => new
            {
                e.Id,
                e.FullName,
                e.Email,
                Department = e.Department!.Name,
                Position = e.Position!.Name
            })
            .ToListAsync();

        return Ok(data);
    }
}
