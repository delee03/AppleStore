using AppleStore.DataAcess;
using AppleStore.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AppleStore.Controllers
{
    public class StoreController : Controller
    {
		private readonly IProductRepository _productRepository;

		public StoreController(IProductRepository productRepository, ICategoryRepository categoryRepository, ApplicationDbContext dbContext)
		{
			_productRepository = productRepository;
		}
		public async Task<IActionResult> Index()
        {
			var products = await _productRepository.GetAllAsync();
			ViewBag.Products = products;
			return View();
        }
    }
}
