using Microsoft.AspNetCore.Mvc;
using AppleStore.DataQuery;
using AppleStore.Models;
using AppleStore.DataAcess;
using AppleStore.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace AppleStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ApplicationDbContext _context;

        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository, ApplicationDbContext dbContext)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _context = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            ViewBag.Products = products.ToList();
            return View(products);
        }

        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            //Debug.WriteLine("categories.Count: " + categories.Count());
            ViewBag.Categories = categories.ToList();
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
            }

            var categories = await _productRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }

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

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = categories.ToList();
            ViewBag.Product = product;
            ViewBag.dsImage = _context.ProductImages.Where(x => x.ProductId == id).ToList();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, int id, IFormFile imageUrl, List<IFormFile> imageUrls)
        {
            ModelState.Remove("ImageUrl");
            if (id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingProduct = await _productRepository.GetByIdAsync(id);
                if (imageUrl == null)
                {
                    product.ImageUrl = existingProduct.ImageUrl;
                }
                else
                {
                    product.ImageUrl = await SaveImage(imageUrl);
                }

                await UpdateProductImages(id, imageUrls);

                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImageUrl = product.ImageUrl;
                await _productRepository.UpdateAsync(existingProduct);
                return RedirectToAction("Index");

            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = categories.ToList();
            return View(product);
        }

        private async Task UpdateProductImages(int productId, List<IFormFile> newImages)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return;
            }

            var existingImages = _context.ProductImages.Where(x => x.ProductId == productId);
            _context.ProductImages.RemoveRange(existingImages);
            await _context.SaveChangesAsync();

            if (newImages != null)
            {
                foreach (var img in newImages)
                {
                    ProductImage productImage = new ProductImage()
                    {
                        ProductId = product.Id,
                        Url = await SaveImage(img)
                    };
                    _context.ProductImages.Add(productImage);
                }
                await _context.SaveChangesAsync();
            }
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/img", image.FileName);

            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
                return "/img/" + image.FileName;
            }
        }

        private async Task<List<string>> SaveListImage(List<IFormFile> images)
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
