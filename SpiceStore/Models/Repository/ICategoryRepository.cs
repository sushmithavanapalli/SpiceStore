using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpiceStore.Models.Repository
{
    public interface ICategoryRepository
    {
        Task<int> AddNewCategory(Category model);
        Task<List<Category>> GetAllCategories();
    }
}