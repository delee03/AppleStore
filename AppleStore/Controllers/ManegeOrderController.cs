/*using AppleStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AppleStore.Controllers
{
   
    public class ManageOrderController : Controller
    {
        private readonly IOrder _orderRepo;
        private readonly IOrderDetails _orderDetailsRepo;
        private readonly WebsiteSellingComputerDbContext _websiteSellingComputerDbContext;
        public ManageOrderController(IOrder orderRepo, IOrderDetails orderDetailsRepo, WebsiteSellingComputerDbContext websiteSellingComputerDbContext)
        {
            _orderRepo = orderRepo;
            _orderDetailsRepo = orderDetailsRepo;
            _websiteSellingComputerDbContext = websiteSellingComputerDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var orderList = await _orderRepo.GetAllAsync();
            return View(orderList);
        }
        public async Task<IActionResult> Details(int id)
        {
            var ordersDetailsList = await _orderDetailsRepo.GetOrderDetailsByIdAsync(id);
            if (ordersDetailsList != null)
            {
                return View(ordersDetailsList);
            }
            else
                return NoContent();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}

*/