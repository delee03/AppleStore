using Microsoft.AspNetCore.Mvc;

namespace AppleStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.Remove("admin");
            return RedirectToAction("Index","Login");
        }
    }
}
