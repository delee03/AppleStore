﻿using System.Reflection.Metadata.Ecma335;
using AppleStore.DataAcess;
using AppleStore.Models;
using AppleStore.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AppleStore.Controllers
{
	
	public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ApplicationDbContext _context;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, ApplicationDbContext dbContext)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _context = dbContext;
        }

		//Loc du lieu va tim kiem
		[HttpGet]
        public async Task<IActionResult> Index(string name, decimal? Priceto, decimal? Pricefrom, int? CategoryId )
        {
            var products = await _productRepository.GetAllAsync();
			var categories = await _categoryRepository.GetAllAsync();
			var listCategories = categories.ToList();
			listCategories.Insert(0, new Category { Id = 0, Name = "Select Category" });
			ViewBag.CategoryID = new SelectList(listCategories, "Id", "Name", CategoryId);
			if (!string.IsNullOrEmpty(name))
            {
                if (Priceto != null && Pricefrom != null)
                {
                    if(CategoryId > 0)
                    {
						products = products.Where(x => x.CategoryId == CategoryId && x.Name.Contains(name) && x.Price >= Priceto && x.Price <= Pricefrom);

					}
                    else
                    {
						products = products.Where(x => x.Name.Contains(name) && x.Price >= Priceto && x.Price <= Pricefrom);

					}

				}
                else 
                {
                    products = products.Where(x => x.Name.Contains(name));

                }
            }
            else
			{
				if (Priceto != null && Pricefrom != null)
				{
					if ( CategoryId > 0)
					{
						products = products.Where(x => x.CategoryId == CategoryId && x.Price >= Priceto && x.Price <= Pricefrom);

					}
					else
					{
						products = products.Where(x => x.Price >= Priceto && x.Price <= Pricefrom);
					}
				}
				else
				{
					products = products.Where(x => x.CategoryId == CategoryId);
				}				
			
			}
           
			return View(products);
        }


		/*  public async Task<ActionResult> ShowCategories(int? CategoryId)
		  {
			  CategoryId = CategoryId ?? 0;
			  var categories = await _categoryRepository.GetAllAsync();
			  var listCategories = categories.ToList();
			  listCategories.Insert( 0, new Category { Id = 0, Name = "--Select Category--" });
			  ViewBag.CategoryID = new SelectList(listCategories, "Id", "Name", CategoryId);
			  var products =_context.Products.Where(x => x.CategoryId == CategoryId);
			  return View(products);
		  }*/
		
		
		
		public async Task<ActionResult> ShowIphone(string name, decimal? Priceto, decimal? Pricefrom)
		{
			
			var products = await _productRepository.GetAllAsync();
			
			if (!string.IsNullOrEmpty(name))
			{
				if (Priceto != null && Pricefrom != null)
				{
					products = products.Where(x => x.CategoryId == 1 && x.Name.Contains(name) && x.Price >= Priceto && x.Price <= Pricefrom);

				}
				else
				{
					products = products.Where(x => x.CategoryId == 1 && x.Name.Contains(name));

				}
			}
			else
			{
				if (Priceto != null && Pricefrom != null)
				{
					products = products.Where(x => x.CategoryId == 1 && x.Price >= Priceto && x.Price <= Pricefrom);

				}
				else
				{
					products = _context.Products.Where(x => x.CategoryId == 1);
				}
			}
			return View(products);
			
		}
		
		public async Task<ActionResult> ShowMacbook(string name, decimal? Priceto, decimal? Pricefrom)
		{
			var products = await _productRepository.GetAllAsync();

			if (!string.IsNullOrEmpty(name))
			{
				if (Priceto != null && Pricefrom != null)
				{
					products = products.Where(x => x.CategoryId == 2 && x.Name.Contains(name) && x.Price >= Priceto && x.Price <= Pricefrom);

				}
				else
				{
					products = products.Where(x => x.CategoryId == 2 && x.Name.Contains(name));

				}
			}
			else
			{
				if (Priceto != null && Pricefrom != null)
				{
					products = products.Where(x => x.CategoryId == 2 && x.Price >= Priceto && x.Price <= Pricefrom);

				}
				else
				{
					products = _context.Products.Where(x => x.CategoryId == 2);
				}
			}
			return View(products);

		}
		[HttpGet]
		public async Task<ActionResult> ShowAccessories(string name, decimal? Priceto, decimal? Pricefrom)
		{
			var product = await _productRepository.GetAllAsync();
			var products = await _productRepository.GetAllAsync();

			if (!string.IsNullOrEmpty(name))
			{
				if (Priceto != null && Pricefrom != null)
				{
					products = products.Where(x => x.CategoryId == 3 && x.Name.Contains(name) && x.Price >= Priceto && x.Price <= Pricefrom);

				}
				else
				{
					products = products.Where(x => x.CategoryId == 3 && x.Name.Contains(name));

				}
			}
			else
			{
				if (Priceto != null && Pricefrom != null)
				{
					products = products.Where(x => x.CategoryId == 3 && x.Price >= Priceto && x.Price <= Pricefrom);

				}
				else
				{
					products = _context.Products.Where(x => x.CategoryId == 3);
				}
			}
			return View(products);

		}

		//Tạo form thêm sản phẩm mới
		public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    product.ImageUrl = await SaveImage(imageUrl);

                }
                if (imageUrls != null)
                {
                    product.ImageUrls = new List<ProductImage>();
                    foreach (var img in imageUrls)
                    {
                        ProductImage productImage = new ProductImage()
                        {
                            ProductId = product.Id,
                            Url = await SaveImage(img)
                        };
                        product.ImageUrls.Add(productImage);
                    }
                }
                await _productRepository.AddAsync(product);
                return RedirectToAction(nameof(Index));
                //nếu modelstate ko hợp lệ hiển thị form với dữ liệu đã nhập            

            }

            var categories = await _productRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }
        // Display a single product
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.dsImage = _context.ProductImages.Where(x => x.ProductId == id).ToList();
            return View(product);
        }
		//Show the product update form

		public async Task<IActionResult> Update(int id)
		{
			var product = await _productRepository.GetByIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			var categories = await _categoryRepository.GetAllAsync();
			ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
			ViewBag.dsImage = _context.ProductImages.Where(x => x.ProductId == id).ToList();
			return View(product);
		}

		// Quá trình cập nhật
		[HttpPost]
		public async Task<IActionResult> Update(Product product, int id, IFormFile imageUrl)
		{
			ModelState.Remove("ImageUrl");//Loại bỏ xác thực ModelState cho ImageUrl
			if (id != product.Id)
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{
				var existingProduct = await _productRepository.GetByIdAsync(id);
				//giả định có phương thức getbyidasync
				//Giữ nguyên thông tin hình ảnh nếu không có hình mới đc tải lên
				if (imageUrl == null)
				{
					product.ImageUrl = existingProduct.ImageUrl;
					//product.ImageUrls = existingProduct.ImageUrls;
				}
				else
				{
					//lưu hình ảnh mới
					product.ImageUrl = await SaveImage(imageUrl);
					//     product.ImageUrls = await SaveListImage(imageUrls);
					/*  product.ImageUrls = new List<ProductImage>();
					  foreach (var img in imageUrls)
					  {
						  ProductImage productImage = new ProductImage()
						  {
							  ProductId = product.Id,
							  Url = await SaveImage(img)
						  };
						  product.ImageUrls.Add(productImage);
					  }*/

				}
				existingProduct.Name = product.Name;
				existingProduct.Price = product.Price;
				existingProduct.Description = product.Description;
				existingProduct.CategoryId = product.CategoryId;
				existingProduct.ImageUrl = product.ImageUrl;
				existingProduct.ImageUrls = product.ImageUrls;
				await _productRepository.UpdateAsync(existingProduct);
				return RedirectToAction(nameof(Index));

			}
			var categories = await _categoryRepository.GetAllAsync();
			ViewBag.Categories = new SelectList(categories, "Id", "Name");
			//ViewBag.dsImage = _context.ProductImages.Where(x => x.ProductId == id).ToList();
			return View(product);
		}

		// Show the product delete confirmation
		/* [Authorize(Roles = "Admin")]*/
		public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        //Process the product deletion
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

		private async Task<string> SaveImage(IFormFile image)
		{
			var savePath = Path.Combine("wwwroot/img", image.FileName);
			//thay doi duong dan theo cau hinh cua bn
			using (var fileStream = new FileStream(savePath, FileMode.Create))
			{
				await image.CopyToAsync(fileStream);
			}
			return "/img/" + image.FileName;
			//tra ve duong dan tuong doi
		}


        private async Task<List<string>> SaveListImage(List<FormFile> images)
        {
            var savedPaths = new List<string>();
            var basePath = "wwwroot/img"; // Adjust this path according to your project structure

            foreach (var formFile in images)
            {
                var fileName = Path.GetFileName(formFile.FileName);
                var savePath = Path.Combine(basePath, fileName);

                using (var fileStream = new FileStream(savePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }

                savedPaths.Add("/img/" + fileName);
            }

            return savedPaths;
        }
    }
}
