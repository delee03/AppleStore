using AppleStore.Models;
using AppleStore.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AppleStore.Extensions;
using Microsoft.AspNetCore.Authorization;
using AppleStore.DataAcess;
using Newtonsoft.Json;
using static AppleStore.Models.ShoppingCart;

namespace AppleStore.Controllers
{
	public class ShoppingCartController : Controller
	{
		private readonly IProductRepository _productRepository;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		public ShoppingCartController( ApplicationDbContext context,UserManager<ApplicationUser> userManager, IProductRepository productRepository)
		{
			_productRepository = productRepository;
			_context = context;
			_userManager = userManager;
		}	
		public IActionResult Index()
		{
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
			return View(cart);
		}

		public async Task<IActionResult> AddToCart(int productId, int quantity)
		{
			// Giả sử bạn có phương thức lấy thông tin sản phẩm từ productId
			var product = await GetProductFromDatabase(productId);
			var cartItem = new CartItem
			{
				ProductId = productId,
				Name = product.Name,
				Price = product.Price,
				Quantity = quantity
			};
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
			cart.AddItem(cartItem);
			HttpContext.Session.SetObjectAsJson("Cart", cart);
			//return RedirectToAction("Index");
			return Json(new { success = true });
		}


        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();

            // Update the quantity of the specified product in the cart
            foreach (var item in cart.Items)
            {
                if (item.ProductId == productId)
                {
                    if (item.Quantity == 1)
                    {
                        cart.RemoveItem(item.ProductId);
                        HttpContext.Session.SetObjectAsJson("Cart", cart);
                        break;
                    }


                    item.Quantity += quantity;
                    break;
                }
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index"); // Assuming you have an "Index" action to display the updated cart
        }

        public IActionResult UpdateInsQuantity(int productId, int quantity)
		{
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();

			// Update the quantity of the specified product in the cart
			foreach (var item in cart.Items)
			{
				if (item.ProductId == productId)
				{
					if (item.Quantity < 1)
					{
						cart.RemoveItem(item.ProductId);
						HttpContext.Session.SetObjectAsJson("Cart", cart);
						break;
					}


					item.Quantity += quantity;
					break;
				}
			}

			HttpContext.Session.SetObjectAsJson("Cart", cart);
			return RedirectToAction("Index"); // Assuming you have an "Index" action to display the updated cart
		}



		// Các actions khác...
		private async Task<Product> GetProductFromDatabase(int productId)
		{
			// Truy vấn cơ sở dữ liệu để lấy thông tin sản phẩm
			var product = await _productRepository.GetByIdAsync(productId);
			return product;
		}

		public IActionResult RemoveFromCart(int productId)
		{
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
			if (cart is not null)
			{
				cart.RemoveItem(productId);
				// Lưu lại giỏ hàng vào Session sau khi đã xóa mục
				HttpContext.Session.SetObjectAsJson("Cart", cart);
			}

			return RedirectToAction("Index");
		}


		public IActionResult RemoveAllFromCart()
        {

            var newCart = new ShoppingCart();
            // Lưu lại giỏ hàng vào Session sau khi đã xóa mục
            HttpContext.Session.SetObjectAsJson("Cart", newCart);


            return RedirectToAction("Index");
        }

		[Authorize]
		public IActionResult Checkout()
		{
			List<CartItem> cartItems = new List<CartItem>();
			ViewBag.CartItems = cartItems;
			return View(new Order());
		}

		[HttpPost]
		public async Task<IActionResult> Checkout(Order order)
		{
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
			if (cart == null || !cart.Items.Any())
			{
				// Xử lý giỏ hàng trống...
				return RedirectToAction("Index");
			}

			var user = await _userManager.GetUserAsync(User);
			order.UserId = user.Id;
			order.OrderDate = DateTime.UtcNow;
			order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
			order.OrderDetails = cart.Items.Select(i => new OrderDetail
			{
				ProductId = i.ProductId,
				Quantity = i.Quantity,
				Price = i.Price
			}).ToList();
			_context.Orders.Add(order);
			await _context.SaveChangesAsync();
			HttpContext.Session.Remove("Cart");
			return View("OrderCompleted", order.Id); 
		}
	}
}
