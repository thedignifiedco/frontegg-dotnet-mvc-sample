using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fronteggnetmvcsample.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public IActionResult Portal()
        {
            return View();
        }
    }
}
