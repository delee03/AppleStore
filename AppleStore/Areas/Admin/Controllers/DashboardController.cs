using Microsoft.AspNetCore.Mvc;

namespace AppleStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            // test.
            //HttpContext.Session.SetString("admin", "1");
            return View();
        }

        public IActionResult Tables()
        {
            return View("tables");
        }

        public IActionResult Billing()
        {
            return View("billing");
        }

        public IActionResult Notifications()
        {
            return View("notifications");
        }

        public IActionResult Profile()
        {
            return View("profile");
        }

        public IActionResult Signup()
        {
            return View("signup");
        }
    }
}
