using Microsoft.AspNetCore.Mvc;

namespace AppleStore.Controllers
{
    public class BlankController : Controller
    {
        public IActionResult Index()
        {
            return View("Blank");
        }
    }
}
