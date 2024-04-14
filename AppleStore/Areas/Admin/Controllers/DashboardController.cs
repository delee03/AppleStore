using AppleStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppleStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            // test.
            //HttpContext.Session.SetString("admin", "1");
            var Customers = await _userManager.GetUsersInRoleAsync("Customer");
            ViewBag.CustomersCount = Customers.Count();
            return View();
        }
    }
}
