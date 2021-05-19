using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpiceStore.Data;
using SpiceStore.Models;
using SpiceStore.Models.Repository;

namespace SpiceStore.Controllers
{
    public class SubCategoryController : Controller
    {
        private readonly ISubCategoryRepository subCategoryRepository = null;
        private readonly ICategoryRepository categoryRepository = null;
        private readonly SpiceStoreContext _context = null;
        public SubCategoryController(SpiceStoreContext spiceStoreContext, ISubCategoryRepository _subcategoryRepository, ICategoryRepository _categoryRepository)
        {
            subCategoryRepository = _subcategoryRepository;
            categoryRepository = _categoryRepository;
            _context = spiceStoreContext;
        }
        [Route("SubCategory")]
        public async Task<IActionResult> SubCatList()
        {
            var subCategories = await subCategoryRepository.GetSubCategories();
            ViewData["Categories"] = await categoryRepository.GetAllCategories();
            return View(subCategories);
        }


        [Route("Create SubCategory")]
        public async Task<ViewResult> CreateSubCat(bool msg, string err)
        {
            ViewData["subCategories"] = await subCategoryRepository.GetSubCategories();
            ViewData["Categories"] = await categoryRepository.GetAllCategories();

            if (msg == true && err == null)
            {
                ViewBag.Message = "Added a new Sub Category";
            }
            else if(msg == false && err != null){
                ViewBag.Alert = err;
            }
            else
            {
                ViewBag.Message = null;
            }

            PopulateCategoryDropDownList();

            return View();
        }


        [Route("Create SubCategory")]
        [HttpPost]
        public async Task<IActionResult> CreateSubCat(SubCategory viewModel)
        {
            if (ModelState.IsValid)
            {
                var data = _context.SubCategories.Where(s => s.CategoryKey == viewModel.CategoryKey && s.Subcategory == viewModel.Subcategory).FirstOrDefault();

                if (data != null)
                {
                    return RedirectToAction("CreateSubCat", new { msg = false, err = "Combination already exists" });
                }
                else
                {
                    _context.SubCategories.Add(new SubCategory()
                    {
                        Subcategory = viewModel.Subcategory,
                        CategoryKey = viewModel.CategoryKey
                    });
                    await _context.SaveChangesAsync();
                    return RedirectToAction("CreateSubCat", new { msg = true, err = string.Empty });
                }
                
            }
            PopulateCategoryDropDownList();
            return View();
        }


        [Route("Delete SubCategory")]
        //Delete a subcategory
        public async Task<IActionResult> Delete(int id)
        {
            var data = _context.SubCategories.Where(s => s.SubCategoryKey == id).FirstOrDefault();
            _context.SubCategories.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToAction("SubCatList");
        }


        [Route("Update SubCategory/{id:int})")]
        //Update existing SubCategory
        public async Task<ViewResult> UpdateSubCat(int id, bool msg, string err)
        {
            ViewData["subCategories"] = await subCategoryRepository.GetSubCategories();
            ViewData["Categories"] = await categoryRepository.GetAllCategories();

            if (msg == true && err == null)
            {
                ViewBag.Message = "Successfully Updated SubCategory";
            }
            else if (msg == false && err != null)
            {
                ViewBag.Alert = err;
            }
            else
            {
                ViewBag.Message = null;
            }
            
            SubCategory model = new SubCategory();
            if(id > 0)
            {
                var subCat = _context.SubCategories.Where(s => s.SubCategoryKey == id).FirstOrDefault();
                if (subCat != null)
                {
                    model.CategoryKey = subCat.CategoryKey;
                    model.Categorycode = subCat.Categorycode;
                    model.Subcategory = subCat.Subcategory;
                    model.SubCategoryKey = subCat.SubCategoryKey;
                }
                                
                PopulateCategoryDropDownList(subCat.CategoryKey);
            }
                                
            return View(model);
        }


        [Route("Update SubCategory/{id:int})")]
        [HttpPost]
        public async Task<IActionResult> UpdateSubCat(int? id, SubCategory model)
        {
            var subCatandCat = _context.SubCategories.Where(s => s.SubCategoryKey == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                var data = _context.SubCategories.Where(s => s.CategoryKey == model.CategoryKey && s.Subcategory == model.Subcategory).FirstOrDefault();

                if (data != null)
                {
                    return RedirectToAction("UpdateSubCat", new { msg = false, err = "Combination already exists" });
                }
                else
                {
                    subCatandCat.CategoryKey = model.CategoryKey;
                    subCatandCat.Categorycode = model.Categorycode;
                    subCatandCat.Subcategory = model.Subcategory;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("UpdateSubCat", new { msg = true, err = string.Empty });
                }
                                
            }
            PopulateCategoryDropDownList(subCatandCat.CategoryKey);
            return RedirectToAction("UpdateSubCat", new { msg = false } );
        }


        //Drop down List of Categories
        private void PopulateCategoryDropDownList(object selectedCategory = null)
        {
            var CategoryQuery = from c in _context.Categories
                                orderby c.CategoryName
                                select c;
            ViewBag.CategoryKey = new SelectList(CategoryQuery, "CategoryKey", "CategoryName", selectedCategory);
        }
    }
}