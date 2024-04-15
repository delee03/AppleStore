using Microsoft.AspNetCore.Mvc;
using AppleStore.Repositories;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AppleStore.DataAcess;

namespace AppleStore.Controllers
{
    [Area("Admin")]
    public class InvoicesController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ApplicationDbContext _context;

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

        public async Task<IActionResult> Detail(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewBag.invoices = _context.Orders.Where(x => x.Id == id).ToList();
            return View(order);
        }

        public async Task<IActionResult> Print(int id)
        {
            var invoices = await _orderRepository.GetOrderByIdAsync(id);
            ViewBag.invoices = invoices;
            return View(invoices);
        }
    }
}