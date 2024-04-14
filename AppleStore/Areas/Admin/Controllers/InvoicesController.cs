using Microsoft.AspNetCore.Mvc;
using AppleStore.Repositories;
using System.Threading.Tasks;

namespace AppleStore.Controllers
{
    [Area("Admin")]
    public class InvoicesController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public InvoicesController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> Index()
        {
            var invoices = await _orderRepository.GetAllOrdersAsync();
            ViewBag.invoices = invoices;
            return View(invoices);
        }

        public async Task<IActionResult> Print(int id)
        {
            var invoices = await _orderRepository.GetOrderByIdAsync(id);
            ViewBag.invoices = invoices;
            return View(invoices);
        }
    }
}