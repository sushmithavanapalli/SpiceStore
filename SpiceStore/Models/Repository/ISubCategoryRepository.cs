using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpiceStore.Models.Repository
{
    public interface ISubCategoryRepository
    {
        Task<List<SubCategory>> GetSubCategories();
        Task<int> AddNewSubCategory(SubCategory model);
    }
}