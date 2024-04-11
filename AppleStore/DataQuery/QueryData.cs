using AppleStore.DataAcess;
using AppleStore.Models;
using Microsoft.EntityFrameworkCore;
using AppleStore.Repository;

namespace AppleStore.DataQuery
{
    public class QueryData
    {
        private readonly ApplicationDbContext? _context;

        public bool loginAdmin(string email, string password)
        {
            if (email.Equals("admin@a.com") && password.Equals("applestore@123")) return true;
            return false;
        }

        public bool loginAdmin(string password)
        {
            if (password.Equals("applestore@123")) return true;
            return false;
        }

        public List<Product> GetProductsWithType(string search = "", int order_by = 1, int category_id = 0, int page = 1, int limit = 20)
        {
            using (var dbContext = _context)
            {

                IQueryable<Product> list = (from p in dbContext.Products
                                            select p).Include(i => i.Category);


                if (!string.IsNullOrEmpty(search))
                    list = list.Where(w => w.Name.ToLower().Contains(search.ToLower()));
                if (category_id > 0)
                    list = list.Where(w => w.CategoryId == category_id);
                //if (product_published == 0 || product_published == 1)
                //    list = list.Where(w => w.product_published == product_published);

                switch (order_by)
                {
                    case 2:
                        list = list.OrderBy(o => o.Price);
                        break;
                    case 3:
                        list = list.OrderByDescending(o => o.Price);
                        break;
                    default:
                        list = list.OrderByDescending(o => o.Id);
                        break;
                }

                if (page <= 0) page = 1;
                if (limit > 0)
                    list = list.Skip((page - 1) * limit).Take(limit);

                return list.ToList();
            }
        }

        public List<Category> GetProductTypes()
        {
            using (var dbContext = _context)
            {
                return dbContext.Categories.ToList();
            }
        }

        public Product? GetProduct(int product_id)
        {
            using (var dbContext = _context)
            {
                return dbContext.Products.SingleOrDefault(s => s.Id == product_id);
            }
        }

        public Product? GetProductWithType(int product_id)
        {
            using (var dbContext = _context)
            {
                return (from p in dbContext.Products
                        where p.Id == product_id
                        select p).Include(i => i.Id).FirstOrDefault();
            }
        }

        public int PostProduct(Product product, out string err)
        {
            err = string.Empty;
            try
            {
                using (var dbContext = _context)
                {
                    dbContext.Products.Add(product);
                    dbContext.SaveChanges();
                    return product.Id;
                }
            }
            catch (Exception e)
            {
                err = e.Message;
                return 0;
            }
        }

    }
}
