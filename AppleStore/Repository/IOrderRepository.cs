using AppleStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppleStore.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<int> GetTotalOrdersCountAsync();
        Task<decimal> GetTotalRevenueAsync();


		// Các phương thức khác như thêm, cập nhật, xóa hóa đơn cũng có thể được khai báo ở đây
	}
}
