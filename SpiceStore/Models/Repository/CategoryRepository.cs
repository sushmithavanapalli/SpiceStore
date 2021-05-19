using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpiceStore.Data;
using Microsoft.EntityFrameworkCore;

namespace SpiceStore.Models.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private SpiceStoreContext _context = null;
        public CategoryRepository(SpiceStoreContext spiceStoreContext)
        {
            _context = spiceStoreContext;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            var Categories = await _context.Categories.ToListAsync();
            var CategoryList = new List<Category>();
            if (Categories.Count > 0)
            {
                foreach (var category in Categories)
                {
                    CategoryList.Add(new Category()
                    {
                        CategoryKey = category.CategoryKey,
                        CategoryName = category.CategoryName
                    });
                }
            }

            return CategoryList;
        }

        //Add new Category
        public async Task<int> AddNewCategory(Category model)
        {
            var newcategory = new Category()
            {
                CategoryName = model.CategoryName
            };
            await _context.Categories.AddAsync(newcategory);
            await _context.SaveChangesAsync();
            return newcategory.CategoryKey;
        }
    }
}
