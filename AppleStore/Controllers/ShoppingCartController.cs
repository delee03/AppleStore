using AppleStore.Models;
using AppleStore.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AppleStore.Extensions;
using Microsoft.AspNetCore.Authorization;
using AppleStore.DataAcess;
using AppleStore.Services;
using AppleStore.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Azure;

namespace AppleStore.Controllers
{
	
	public class ShoppingCartController : Controller
	{
		private readonly IProductRepository _productRepository;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVnPayService _vnPayService;

        public ShoppingCartController( ApplicationDbContext context,
UserManager<ApplicationUser> userManager, IProductRepository productRepository, IVnPayService vnPayService)
		{
			_productRepository = productRepository;
			_context = context;
			_userManager = userManager;
            _vnPayService = vnPayService;
		}	
		public IActionResult Index()
		{
			ViewBag.ProductList = _context.Products.ToList();
            
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
			return View(cart);
		}

        public IActionResult DescQuantity(int productId, int quantity)
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


        public IActionResult AccesQuantity(int productId, int quantity)
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

        [Authorize]
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
        public async Task<IActionResult> Checkout(Order order, string payment = "COD")
        {
            var cart =
               HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            
             if (payment == "thanh toán vnpay")
            {
                var vnPayModel = new VnPaymentRequestModel
                {
                    Amount = (double)cart.Items.Sum(x => x.Quantity * x.Price),
                    CreatedDate = DateTime.Now,
                    Desc = $"{order.FullName_Order} {order.PhoneNumber_Order}",
                    FullName = order.FullName_Order,
                    OrderId = new Random().Next(1000, 10000)
                };
                return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));

            }
            TempData["MessageCOD"] = payment;

            if (cart == null || !cart.Items.Any())
            {
                // Xử lý giỏ hàng trống...
                return RedirectToAction("/Product");
            }
            var user = await _userManager.GetUserAsync(User);          
            ViewBag.Info = order;
            order.UserId = user.Id;
            order.OrderDate = DateTime.Now;
            /* order.PhoneNumber_Order = users.PhoneNumber;
             order.FullName_Order = users.FullName;
             order.ShippingAddress = users.Address;
             order.Email_Order = users.Email;*/
            order.Vnpay_transaction = "COD"; 
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
            TempData["OrderId"] = order.Id;

            return View("SucessfulOrderCOD", order.OrderDate);
        }
        [Authorize]
        public IActionResult PaymentFail()
        {
            return View();
        }
        //hàm trả về sucessfulOrer VNPay và lưu vào database
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PaymentCallBack(Order order)
        {
            var cart =
             HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");


            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VNPay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }
            //Lưu đơn hàng vào database tự code
            var user = await _userManager.GetUserAsync(User);
            ViewBag.Info = order;
            order.UserId = user.Id;
            order.OrderDate = DateTime.Now;
            /* order.PhoneNumber_Order = users.PhoneNumber;
             order.FullName_Order = users.FullName;
             order.ShippingAddress = users.Address;
             order.Email_Order = users.Email;*/
            order.Vnpay_transaction = "COD";
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
            TempData["Message"] = $"Thanh toán VNPAY thành công: {response.VnPayResponseCode}";
            TempData["OrderId"] = response.OrderId;
            TempData["Desc"] = response.OrderDescription;           

          
            return View("SucessfulOrder");

        }



    }
}
