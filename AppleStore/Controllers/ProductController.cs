using Microsoft.AspNetCore.Mvc;

namespace AppleStore.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View("product");
        }
    }
}
