using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpiceStore.Models;

namespace SpiceStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> _user, SignInManager<ApplicationUser> signIn)
        {
            userManager = _user;
            signInManager = signIn;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ViewResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel user)
        {
            string role = Request.Form["roleName"].ToString();
            if (ModelState.IsValid)
            {
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
                if (!result.Succeeded)
                {
                    foreach(var res in result.Errors)
                    {
                        ModelState.AddModelError("", res.Description);
                    }
                }
                else
                {
                    if (result.Succeeded)
                    {
                        var res = await userManager.AddToRoleAsync(NewUser, role);
                    }
                }
                ModelState.Clear();
            }
            return View();
        }
      
        public ViewResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel userModel,string returnUrl)
        {            
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(userModel.Email, userModel.Password, userModel.Remember, false);
                if (result.Succeeded)
                {                   
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid Credentials");
            }
            
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
