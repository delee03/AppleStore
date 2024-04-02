using Microsoft.AspNetCore.Mvc;

namespace AppleStore.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View("Checkout");
        }
    }
}
