using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpiceStore.Models;
using SpiceStore.Data;
using SpiceStore.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace SpiceStore.Controllers
{
    public class CouponController : Controller
    {
        private readonly SpiceStoreContext storeContext = null;
         public CouponController(SpiceStoreContext _context)
        {
            storeContext = _context;
        }
        public IActionResult Index()
        {
            return View();
        }

        //Create Coupon
        public IActionResult CreateCoupon(bool msg)
        {
            if(msg == true)
            {
                ViewBag.Message = "Successfully created a coupon";
            }
            else
            {
                ViewBag.Message = null;
            }
            return View();
        }

        //Post Method of Create Coupon
        [HttpPost]
        public async Task<IActionResult> CreateCoupon(Coupon model)
        {
            if (ModelState.IsValid)
            {
                Coupon _coupon = new Coupon()
                {
                    coupontype = model.coupontype,
                    Discount = model.Discount,
                    Name = model.Name,
                    MinAmnt = model.MinAmnt,
                    isActive = model.isActive
                };
                await storeContext.Coupon.AddAsync(_coupon);
                await storeContext.SaveChangesAsync();
                return RedirectToAction("CreateCoupon", new { msg = true });
            }
            return RedirectToAction("CreateCOupon", new { msg = false });
        }

        //List all Coupons
        public async Task<IActionResult> ViewCoupon()
        {
            var couponList = await storeContext.Coupon.ToListAsync();
            return View(couponList); 
        }

        //Update Coupon
        public IActionResult updateCoupon(int? id, bool msg)
        {
            if(msg == true)
            {
                ViewBag.UpdateMsg = "Successfully updated Coupon";
            }
            else
            {
                ViewBag.UpdateMsg = null;
            }
            var couponItem = storeContext.Coupon.Where(c => c.CouponKey == id).FirstOrDefault();
            Coupon model = new Coupon()
            {
                coupontype = couponItem.coupontype,
                Discount = couponItem.Discount,
                Name = couponItem.Name,
                MinAmnt = couponItem.MinAmnt,
                isActive = couponItem.isActive
            };
            return View(model);
        }

        //Post method of Update Coupon
        [HttpPost]
        public async Task<IActionResult> updateCoupon(int? id, Coupon model)
        {
            if (ModelState.IsValid)
            {
                var couponItem = storeContext.Coupon.Where(c => c.CouponKey == id).FirstOrDefault();
                if (couponItem != null)
                {
                    couponItem.coupontype = model.coupontype;
                    couponItem.Discount = model.Discount;
                    couponItem.isActive = model.isActive;
                    couponItem.MinAmnt = model.MinAmnt;
                    couponItem.Name = model.Name;
                    await storeContext.SaveChangesAsync();

                    return RedirectToAction("updateCoupon", new { msg = true });
                }
            }
            return RedirectToAction("updateCoupon", new { msg = false });
        }

        //Delete Coupon
        public async Task<IActionResult> deleteCoupon(int id)
        {
            var couponitem = storeContext.Coupon.Where(c => c.CouponKey == id).FirstOrDefault();
            storeContext.Coupon.Remove(couponitem);
            await storeContext.SaveChangesAsync();
            return RedirectToAction("ViewCoupon");
        }
    }
}
