using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SpiceStore.Data;
using SpiceStore.Models.Repository;
using SpiceStore.Models.ViewModels;
using SpiceStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace SpiceStore.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly ISubCategoryRepository subCategoryRepository = null;
        private readonly ICategoryRepository categoryRepository = null;
        public readonly SpiceStoreContext storeContext = null;
        private readonly IWebHostEnvironment webHostEnvironment;

        public MenuItemController(SpiceStoreContext _context, ISubCategoryRepository subCatRepository, ICategoryRepository _categoryRepository, IWebHostEnvironment hostEnvironment)
        {
            storeContext = _context;
            categoryRepository = _categoryRepository;
            subCategoryRepository = subCatRepository;
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Create a New Menu Item
        [Route("CreateMenuItem")]
        public ViewResult CreateMenu(bool msg)
        {
            if(msg == true)
            {
                ViewBag.Message = "Successfully added Menu Item";
            }
            else
            {
                ViewBag.Message = null;
            }
            PopulateCategoryDropDownList();
            return View();
        }

        [Route("CreateMenuItem")]
        [HttpPost]
        public async Task<IActionResult> CreateMenu(MenuItemViewModel model)
        {
            if(ModelState.IsValid){
                var uploadPath = "Images/";
                string MenuImg = await UploadFile(uploadPath, model.MenuImage);
                MenuItem menu = new MenuItem()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    CategoryKey = model.CategoryKey,
                    SubCategoryKey = model.SubCategoryKey,
                    Categorycode = model.Categorycode,
                    SubCategoryCode = model.SubCategoryCode,
                    MenuImgUrl = MenuImg,
                    spicy = model.spicy
                };
                
                storeContext.Add(menu);
                await storeContext.SaveChangesAsync();
                return RedirectToAction("CreateMenu", new { msg = true });
            }

            PopulateCategoryDropDownList();
            return View();
        }

        //View all Menu Items
        public async Task<ViewResult> MenuList()
        {
            var oMenuItemList = await storeContext.MenuItem.ToListAsync();
            ViewData["MenuCategories"] = await storeContext.Categories.ToListAsync();
            ViewData["MenuSubCategories"] = await storeContext.SubCategories.ToListAsync();
            return View(oMenuItemList);
        }

        //Update Menu Item
        public async Task<ViewResult> UpdateMenu(int id, bool msg)
        {
            ViewData["MenuCategories"] = await storeContext.Categories.ToListAsync();
            ViewData["MenuSubCategories"] = await storeContext.SubCategories.ToListAsync();

            if(msg == true)
            {
                ViewBag.Message = "Successfully Updated Menu Item";
            }
            else
            {
                ViewBag.Message = null;
            }

            MenuItem model = new MenuItem();
            if (id > 0)
            {
                var menuItem = storeContext.MenuItem.Where(m => m.MenuKey == id).FirstOrDefault();
                if (menuItem != null)
                {
                    model.MenuKey = menuItem.MenuKey;
                    model.Categorycode = menuItem.Categorycode;
                    model.CategoryKey = menuItem.CategoryKey;
                    model.Description = menuItem.Description;
                    model.MenuImgUrl = menuItem.MenuImgUrl;
                    model.Name = menuItem.Name;
                    model.Price = menuItem.Price;
                    model.SubCategoryCode = menuItem.SubCategoryCode;
                    model.SubCategoryKey = menuItem.SubCategoryKey;
                    model.spicy = menuItem.spicy;
                }
                PopulateCategoryDropDownList(menuItem.CategoryKey);
                PopulateSubCategoryDDL(menuItem.SubCategoryKey, menuItem.CategoryKey);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMenu(int? id, MenuItem menu)
        {
            var menuItem = storeContext.MenuItem.Where(m => m.MenuKey == id).FirstOrDefault();
            if (menuItem != null)
            {
                menuItem.Categorycode = menu.Categorycode;
                menuItem.CategoryKey = menu.CategoryKey;
                menuItem.Description = menu.Description;
                menuItem.MenuImgUrl = menu.MenuImgUrl;
                menuItem.Name = menu.Name;
                menuItem.Price = menu.Price;
                menuItem.SubCategoryCode = menu.SubCategoryCode;
                menuItem.SubCategoryKey = menu.SubCategoryKey;
                menuItem.spicy = menu.spicy;
                await storeContext.SaveChangesAsync();
                return RedirectToAction("UpdateMenu", new { msg = true });
            }
            PopulateCategoryDropDownList(menuItem.CategoryKey);
            PopulateSubCategoryDDL(menuItem.SubCategoryKey, menuItem.CategoryKey);
            return RedirectToAction("UpdateMenu", new { msg = false });
        }

        //Delete Menu Item
        public async Task<IActionResult> DeleteMenu(int id)
        {
            var menuItem = storeContext.MenuItem.Where(m => m.MenuKey == id).FirstOrDefault();
            storeContext.MenuItem.Remove(menuItem);
            await storeContext.SaveChangesAsync();
            return RedirectToAction("MenuList");
        }


        //Upload File to wwwroot/Images
        private async Task<string> UploadFile(string folderpath, IFormFile file)
        {
            folderpath += Guid.NewGuid().ToString() + "_" + file.FileName;
            var uploads = Path.Combine(webHostEnvironment.WebRootPath, folderpath);
            await file.CopyToAsync(new FileStream(uploads, FileMode.Create));
            return "/" + folderpath;
        }

        [HttpPost]
        public JsonResult GetSubCategory(string id)
        {
            int cid;
            List<SelectListItem> SubCategoryNames = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(id))
            {
                cid = Convert.ToInt32(id);
                List<SubCategory> subCategories = storeContext.SubCategories.Where(s => s.CategoryKey == cid).ToList();
                subCategories.ForEach(x =>
                {
                    SubCategoryNames.Add(new SelectListItem { Text = x.Subcategory, Value = x.SubCategoryKey.ToString() });
                });
            }
            //ViewBag.SubCategory = SubCategoryNames;
            return Json(SubCategoryNames);
        }

        //Populate all Categories
        private void PopulateCategoryDropDownList(object selectedCategory = null)
        {
            var CategoryQuery = from c in storeContext.Categories
                                orderby c.CategoryName
                                select c;
            ViewBag.CategoryKey = new SelectList(CategoryQuery, "CategoryKey", "CategoryName", selectedCategory);
        }

        //Populate SubCategories based on selected Category
        private void PopulateSubCategoryDDL(object selectedSubCategory = null, int catKey = 0)
        {
            var SubCategoryQuery = from s in storeContext.SubCategories
                                   where s.CategoryKey == catKey
                                   orderby s.Subcategory
                                   select s;
            ViewBag.SubCategoryKey = new SelectList(SubCategoryQuery, "SubCategoryKey", "Subcategory", selectedSubCategory);
        }


    }
}
