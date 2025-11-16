using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OAuthMvcSample.Data;

namespace OAuthMvcSample.Controllers;

[Authorize]
public class PositionsController : Controller
{
    private readonly AppDbContext _context;

    public PositionsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _context.Positions.ToListAsync();
        return View(list);
    }

    public async Task<IActionResult> Details(int id)
    {
        var pos = await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);
        if (pos == null) return NotFound();
        return View(pos);
    }
}
