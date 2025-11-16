using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OAuthMvcSample.Data;

namespace OAuthMvcSample.Controllers;

[Authorize]
public class DepartmentsController : Controller
{
    private readonly AppDbContext _context;

    public DepartmentsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _context.Departments.ToListAsync();
        return View(list);
    }

    public async Task<IActionResult> Details(int id)
    {
        var dept = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
        if (dept == null) return NotFound();
        return View(dept);
    }
}