using Microsoft.AspNetCore.Mvc;

namespace AppleStore.Controllers
{
    public class StoreController : Controller
    {
        public IActionResult Index()
        {
            return View("Store");
        }
    }
}
