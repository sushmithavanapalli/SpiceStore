using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpiceStore.Data;
using SpiceStore.Models;

namespace SpiceStore.Controllers
{
    public class UserController : Controller
    {
        private SpiceStoreContext _context = null;
        private readonly UserManager<ApplicationUser> userManager;
        public UserController(SpiceStoreContext spiceStoreContext, UserManager<ApplicationUser> _user)
        {
            _context = spiceStoreContext;
            userManager = _user;
        }
        public ViewResult AllUsers()
        {
            var UsersList = userManager.Users.ToList();
            return View(UsersList);
        }
        public async Task<IActionResult> LockUser(string id)
        {
            var user =  _context.AppUsers.Where(u => u.Id == id).FirstOrDefault();
            user.LockoutEnd = DateTime.Now.AddYears(1000);
            await _context.SaveChangesAsync();
            return RedirectToAction("AllUsers");
        }
        public ViewResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterModel user)
        {
            string role = Request.Form["roleName"].ToString();
            var NewUser = new ApplicationUser()
            {
                UserName = user.Email,
                Email = user.Email,
                PhoneNumber = user.Phone,
                Name = user.Name,
                City = user.City,
                PostalCode = user.PostalCode,
                State = user.State,
                StreetAddress = user.StreetAddress
            };
            var result = await userManager.CreateAsync(NewUser, user.Password);
            if (result.Succeeded)
            {
                var res = await userManager.AddToRoleAsync(NewUser, role);
            }
            return View();
        }
    }
}
