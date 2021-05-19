using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceStore.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Coupon> Coupons { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<MenuItem> MenuItems { get; set; }
        public IEnumerable<SubCategory> subCategories { get; set; }
    }
}
