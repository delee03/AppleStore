using AppleStore.Models;

namespace AppleStore.Repositories
{
    public interface ICategoryRepository
    {

        IEnumerable<Category> GetAllCategories();
    }
}
