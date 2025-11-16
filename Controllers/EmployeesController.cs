using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OAuthMvcSample.Data;

namespace OAuthMvcSample.Controllers;

[Authorize]
public class EmployeesController : Controller
{
    private readonly AppDbContext _context;

    public EmployeesController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .ToListAsync();

        return View(list);
    }

    public async Task<IActionResult> Details(int id)
    {
        var emp = await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (emp == null) return NotFound();
        return View(emp);
    }
}
