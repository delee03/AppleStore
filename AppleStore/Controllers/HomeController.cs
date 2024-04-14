using System.Diagnostics;
using AppleStore.Models;
using Microsoft.AspNetCore.Authorization;
using AppleStore.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AppleStore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;


        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
