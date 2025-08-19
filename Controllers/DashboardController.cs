using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fronteggnetmvcsample.Controllers;

[Authorize]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        var claims = User.Claims
            .Select(c => new ClaimView { Type = c.Type, Value = c.Value })
            .OrderBy(c => c.Type)
            .ToList();

        return View(claims);
    }
}

public class ClaimView
{
    public string Type { get; set; } = "";
    public string Value { get; set; } = "";
}
