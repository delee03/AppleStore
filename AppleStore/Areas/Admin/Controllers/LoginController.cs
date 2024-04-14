using AppleStore.DataQuery;
using Microsoft.AspNetCore.Mvc;

namespace AppleStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        QueryData queryData = new QueryData();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(object obj)
        {
            //string username = Convert.ToString(HttpContext.Request.Form["email"]) ?? string.Empty;
            string password = Convert.ToString(HttpContext.Request.Form["password"]) ?? string.Empty;
            if (queryData.loginAdmin(password))
            {
                HttpContext.Session.SetString("admin", "true");
                return RedirectToAction("Index", "DashBoard");
            }
            return View();
        }
    }
}
