using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OAuthMvcSample.Data;
using OAuthMvcSample.Models;

namespace OAuthMvcSample.Controllers;

[Authorize]
public class EmployeeSearchController : Controller
{
    private readonly AppDbContext _context;

    public EmployeeSearchController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var vm = new EmployeeSearchViewModel
        {
            AllDepartments = await _context.Departments.ToListAsync(),
            AllPositions = await _context.Positions.ToListAsync()
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Index(EmployeeSearchViewModel model)
    {
        var query = _context.Employees
            .Include(e => e.Department) // JOIN #1
            .Include(e => e.Position)   // JOIN #2
            .AsQueryable();

        // Пошук за датою/часом
        if (model.HireDateFrom.HasValue)
            query = query.Where(e => e.HireDate >= model.HireDateFrom.Value);

        if (model.HireDateTo.HasValue)
            query = query.Where(e => e.HireDate <= model.HireDateTo.Value);

        // Пошук за списком департаментів
        if (model.DepartmentIds != null && model.DepartmentIds.Any())
            query = query.Where(e => model.DepartmentIds.Contains(e.DepartmentId));

        // Пошук за списком посад
        if (model.PositionIds != null && model.PositionIds.Any())
            query = query.Where(e => model.PositionIds.Contains(e.PositionId));

        // Початок рядка
        if (!string.IsNullOrWhiteSpace(model.NameStartsWith))
            query = query.Where(e => e.FullName.StartsWith(model.NameStartsWith));

        // Кінець рядка
        if (!string.IsNullOrWhiteSpace(model.NameEndsWith))
            query = query.Where(e => e.FullName.EndsWith(model.NameEndsWith));

        model.Results = await query.ToListAsync();

        // Потрібно знову заповнити списки для форми
        model.AllDepartments = await _context.Departments.ToListAsync();
        model.AllPositions = await _context.Positions.ToListAsync();

        return View(model);
    }
}
