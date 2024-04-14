using AppleStore.Models;
using AppleStore.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppleStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderRepository _orderRepository;

        public DashboardController(UserManager<ApplicationUser> userManager, IOrderRepository orderRepository)
        {
            _userManager = userManager;
            _orderRepository = orderRepository;
        }
        public async Task<IActionResult> Index()
        {
            // test.
            //HttpContext.Session.SetString("admin", "1");
            var Customers = await _userManager.GetUsersInRoleAsync("Customer");
            var totalOrdersCount = await _orderRepository.GetTotalOrdersCountAsync();
            var totalRevenue = await _orderRepository.GetTotalRevenueAsync();

            ViewBag.TotalOrdersCount = totalOrdersCount;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.CustomersCount = Customers.Count();
            return View();
        }
    }
}
