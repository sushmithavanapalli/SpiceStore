using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SpiceStore.Models;
using Microsoft.AspNetCore.Identity;

namespace SpiceStore.Data
{
    public class SpiceStoreContext : IdentityDbContext<ApplicationUser>
    {
        //Constructor
        public SpiceStoreContext(DbContextOptions<SpiceStoreContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<MenuItem> MenuItem { get; set; }
        public DbSet<Coupon> Coupon { get; set; }
        public DbSet<ApplicationUser> AppUsers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; } 
    }
}
