using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpiceStore.Models;
using SpiceStore.Models.ViewModels;
using SpiceStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace SpiceStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SpiceStoreContext _context;
        private readonly RoleManager<IdentityRole> roleManager;

        public HomeController(ILogger<HomeController> logger, SpiceStoreContext context, RoleManager<IdentityRole> roleMgr)
        {
            _logger = logger;
            _context = context;
            roleManager = roleMgr;
        }

        public async Task<IActionResult> Index()
        {
            HomeIndexViewModel model = new HomeIndexViewModel();
            model.Categories = await _context.Categories.ToListAsync();
            model.Coupons = await _context.Coupon.ToListAsync();
            model.subCategories = await _context.SubCategories.ToListAsync();
            model.MenuItems = await _context.MenuItem.ToListAsync();
            return View(model);
        }
        public IActionResult GetView(int id)
        {
            ViewData["Categories"] = _context.Categories.ToList();
            //string CategoyName, SubCatName = string.Empty;
            ViewBag.CatName= _context.Categories.Where(c => c.CategoryKey == id).FirstOrDefault().CategoryName;
            
            ViewBag.SubCat = _context.SubCategories.Where(s => s.CategoryKey == id).FirstOrDefault().Subcategory;
            var menuList = new List<MenuItem>();
            HomeIndexViewModel model = new HomeIndexViewModel();
            menuList = _context.MenuItem.Where(m => m.CategoryKey == id).ToList();
            model.MenuItems = menuList;
            return View("~/Views/Home/ChildMenu.cshtml", menuList);
        }

        public IActionResult Privacy()
        {
            return View(roleManager.Roles);
        }
        [Authorize]
        public ViewResult NewRole(bool msg)
        {
            if (msg == true)
            {
                ViewBag.Message = "Added a new Role";
            }
            else
            {
                ViewBag.Message = string.Empty;
            }
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> NewRole([Required]string name)
        {
            var resErr = string.Empty;
            if (ModelState.IsValid)
            {
                IdentityResult res = await roleManager.CreateAsync(new IdentityRole(name));
                if (res.Succeeded)
                {
                    return RedirectToAction("NewRole", new { msg = true });
                }
                else
                {

                    foreach(var result in res.Errors)
                    {
                        resErr += result.Description;
                    }
                }
            }
            ModelState.AddModelError("", resErr);
            return View();
        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Privacy");
                }
            }
            else
            {
                ModelState.AddModelError("", "No role found");
            }
                
            return View("Privacy", roleManager.Roles);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
