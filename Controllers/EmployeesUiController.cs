using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OAuthMvcSample.Controllers;

[Authorize]
public class EmployeesUiController : Controller
{
    public IActionResult Legacy()
    {
        return View();
    }

    public IActionResult New()
    {
        return View();
    }
}