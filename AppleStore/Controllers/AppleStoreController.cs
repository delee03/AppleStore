using Microsoft.AspNetCore.Mvc;

namespace AppleStore.Controllers
{
	public class AppleStoreController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
