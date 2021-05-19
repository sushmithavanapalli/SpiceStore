using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpiceStore.Models.Repository;
using SpiceStore.Data;
using Microsoft.AspNetCore.Http;
using SpiceStore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SpiceStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository = null;
        private SpiceStoreContext _context = null;

        //Constructor
        public CategoryController(ICategoryRepository _categoryRepository, SpiceStoreContext context)
        {
            categoryRepository = _categoryRepository;
            _context = context;
        }

        //Display list of Categories
        [Route("Category")]
        public async Task<IActionResult> CategoryList()
        {
            //Get Category List from Category Repository
            var catList = await categoryRepository.GetAllCategories();
            return View(catList);
        }


        //Update a Category
        [Route("Update Category/{id:int}")]
        public ViewResult Update(int? id, bool? msg)
        {
            Category model = new Category();
            if (id > 0)
            {
                //Get the Category matching id
                Category cat = _context.Categories.Where(cat => cat.CategoryKey == id).FirstOrDefault();
                if(cat != null)
                {
                    model.CategoryKey = cat.CategoryKey;
                    model.CategoryName = cat.CategoryName;
                }
            }
            
            if (msg == true)
            {
                ViewBag.UpdateMsg = "Category Update Successfully!!";
            }
            else
            {
                ViewBag.UpdateMsg = string.Empty;
            }
            return View("~/Views/Category/Update.cshtml", model);
        }


        [HttpPost]
        [Route("Update Category/{id:int}")]
        public async Task<IActionResult> Update(int? id, Category model)
        {
            if (ModelState.IsValid)
            {
                //Get Category matching id
                Category cat = _context.Set<Category>().SingleOrDefault(s => s.CategoryKey == id.Value);
                cat.CategoryName = model.CategoryName;
                await _context.SaveChangesAsync();
                return RedirectToAction("Update", new { msg = true });
            }
            return RedirectToAction("Update", new { msg = false });
        }


        //Delete a Category
        [Route("Delete Category/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var categoryitem = _context.Categories.Where(cat => cat.CategoryKey == id).FirstOrDefault();
            _context.Categories.Remove(categoryitem);
            await _context.SaveChangesAsync();

            return RedirectToAction("CategoryList");
        }


        //Create new Category Action method
        [Route("Create Category")]
        public ViewResult Create(bool msg)
        {
            if(msg == true)
            {
                ViewBag.Message = "Added a new Category";
            }
            else
            {
                ViewBag.Message = string.Empty;
            }
            
            return View();
        }


        [HttpPost]
        [Route("Create Category")]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.CategoryName != null)
                {
                    int id = await categoryRepository.AddNewCategory(category);
                    if(id > 0)
                    {
                        return RedirectToAction("Create", new { msg = true });
                    }
                }
            }

            return View();
        }

    }
}
