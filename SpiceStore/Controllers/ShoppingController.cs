using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpiceStore.Data;
using SpiceStore.Models;
using SpiceStore.Models.ViewModels;

namespace SpiceStore.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly SpiceStoreContext _context;
        public ShoppingController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, SpiceStoreContext context)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            _context = context;
        }

        [Authorize]
        public ViewResult MenuDetails(int Id)
        {
            var MenuItem = _context.MenuItem.Where(m => m.MenuKey == Id).FirstOrDefault();
            ViewBag.MenuUrl = MenuItem.MenuImgUrl;
           
            ShoppingCart shpng = new ShoppingCart()
            {
                MenuItem  = MenuItem ,
                Count = 1,

            };
            PopulateCategoryDropDownList(MenuItem.CategoryKey);
            PopulateSubCategoryDDL(MenuItem.SubCategoryKey, MenuItem.CategoryKey);
            return View(shpng);
        }
        [HttpPost]
        public IActionResult MenuDetails(ShoppingCart shoppingCart)
        {
            return View();
        }
        private void PopulateCategoryDropDownList(object selectedCategory = null)
        {
            var CategoryQuery = from c in _context.Categories
                                orderby c.CategoryName
                                select c;
            ViewBag.CategoryKey = new SelectList(CategoryQuery, "CategoryKey", "CategoryName", selectedCategory);
        }

        //Populate SubCategories based on selected Category
        private void PopulateSubCategoryDDL(object selectedSubCategory = null, int catKey = 0)
        {
            var SubCategoryQuery = from s in _context.SubCategories
                                   where s.CategoryKey == catKey
                                   orderby s.Subcategory
                                   select s;
            ViewBag.SubCategoryKey = new SelectList(SubCategoryQuery, "SubCategoryKey", "Subcategory", selectedSubCategory);
        }
    }
}