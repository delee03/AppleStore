using AppleStore.Models;
using AppleStore.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AppleStore.Extensions;
using Microsoft.AspNetCore.Authorization;
using AppleStore.DataAcess;

namespace AppleStore.Controllers
{
	[Authorize]
	public class ShoppingCartController : Controller
	{
		private readonly IProductRepository _productRepository;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		public ShoppingCartController( ApplicationDbContext context,
UserManager<ApplicationUser> userManager, IProductRepository productRepository)
		{
			_productRepository = productRepository;
			_context = context;
			_userManager = userManager;
		}	
		public IActionResult Index()
		{
			ViewBag.ProductList = _context.Products.ToList();
            
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
				Quantity = quantity,
				ImageUrl = product.ImageUrl,
			};
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
			cart.AddItem(cartItem);

			HttpContext.Session.SetObjectAsJson("Cart", cart);
			return RedirectToAction("Index");
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

        /*	public async Task<IActionResult> Checkout()
            {
                var Customers = await _userManager.GetUsersInRoleAsync("Customer");
                ViewBag.Customers = Customers;
                return View();
            }*/

        /*  public async Task<IActionResult> Checkout(string id)
          {
              var Customers = await _userManager.FindByIdAsync("Customer");
              if(Customers == null)
              {
                  return RedirectToAction("Index");
              }    
              ViewBag.Customers = Customers;
              return View();
          }*/


        public async Task< IActionResult > Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");

            var user =  await _userManager.GetUserAsync(User);
            ViewBag.Email = user.Email;
            ViewBag.FullName = user.FullName;
            //	ViewBag.UserFullName = user.FullName;
            if (cart != null)
            {
                List<CartItem> items = cart.Items;

                foreach (var item in cart.Items)
                {
                    string productName = item.Name;
                    int quantity = item.Quantity;
                    decimal price = item.Price;
                }
                ViewBag.CartOrder = items;
            }
            return View(new Order());
        }




        [HttpPost]
        public async Task<IActionResult> Checkout(Order order, ApplicationUser users)
        {
            var cart =
                HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                // Xử lý giỏ hàng trống...
                return RedirectToAction("/Product");
            }
            var user = await _userManager.GetUserAsync(User);          
            ViewBag.Info = user;
            order.UserId = user.Id;
            order.OrderDate = DateTime.Now;
           /* order.PhoneNumber_Order = users.PhoneNumber;
            order.FullName_Order = users.FullName;
            order.ShippingAddress = users.Address;
            order.Email_Order = users.Email;*/
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


            return View("SucessfulOrder", order.OrderDate);
        }



    }
}
