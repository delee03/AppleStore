using Microsoft.AspNetCore.Mvc;

namespace AppleStore.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View("dashboard");
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

        public IActionResult Signin()
        {
            return View("signin");
        }

        public IActionResult Signup()
        {
            return View("signup");
        }
    }
}
