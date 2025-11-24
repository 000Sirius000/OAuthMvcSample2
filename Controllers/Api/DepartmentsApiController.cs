using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OAuthMvcSample.Data;
using OAuthMvcSample.Models;

namespace OAuthMvcSample.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public DepartmentsApiController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Department>>> GetAll()
        => await _context.Departments.ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Department>> GetById(int id)
    {
        var dept = await _context.Departments.FindAsync(id);
        if (dept == null) return NotFound();
        return dept;
    }

    [HttpPost]
    public async Task<ActionResult<Department>> Create(Department department)
    {
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Department model)
    {
        if (id != model.Id) return BadRequest();
        _context.Entry(model).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var dept = await _context.Departments.FindAsync(id);
        if (dept == null) return NotFound();
        _context.Departments.Remove(dept);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}