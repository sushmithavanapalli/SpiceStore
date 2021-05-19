using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpiceStore.Data;
using Microsoft.EntityFrameworkCore;

namespace SpiceStore.Models.Repository
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private SpiceStoreContext _context = null;
        public SubCategoryRepository(SpiceStoreContext spiceStoreContext)
        {
            _context = spiceStoreContext;
        }
        //List all Sub Categories
        public async Task<List<SubCategory>> GetSubCategories()
        {
            var SubCatList = await _context.SubCategories.ToListAsync();
            var subCategoryList = new List<SubCategory>();
            foreach (var subcat in SubCatList)
            {
                subCategoryList.Add(new SubCategory()
                {
                    CategoryKey = subcat.CategoryKey,
                    Categorycode = subcat.Categorycode,
                    Subcategory = subcat.Subcategory,
                    SubCategoryKey = subcat.SubCategoryKey
                });
            }
            return subCategoryList;
        }
        //Add new Sub Category
        public async Task<int> AddNewSubCategory(SubCategory model)
        {
            var newsubcategory = new SubCategory()
            {
                CategoryKey = model.CategoryKey,
                Categorycode = model.Categorycode,
                Subcategory = model.Subcategory
            };
            await _context.SubCategories.AddAsync(newsubcategory);
            await _context.SaveChangesAsync();
            return newsubcategory.CategoryKey;
        }
       
    }
}
