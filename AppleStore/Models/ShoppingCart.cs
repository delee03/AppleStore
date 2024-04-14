
using AppleStore.DataAcess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppleStore.Models
{
	public class ShoppingCart
	{
		public List<CartItem> Items { get; set; } = new List<CartItem>();

      /*  public users GetUser(int user_id)
        {
            using (var dbContext = new DBContext())
            {
                users x = dbContext.users.SingleOrDefault(s => s.user_id == user_id);
                return x;
            }
        }
        public List<ApplicationUser> GetUsers()
        {
            using (var dbContext = new AspNetUserManager())
            {
                return dbContext.applicationUsers.OrderByDescending(o => o.user_id).ToList();
            }
        }*/

        public void AddItem(CartItem item)
		{
			var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId);


			if (existingItem != null)
			{
				existingItem.Quantity += item.Quantity;
			}
			else
			{
				Items.Add(item);
			}
		}
		public void RemoveItem(int productId)
		{
			Items.RemoveAll(i => i.ProductId == productId);
		}



	}
}