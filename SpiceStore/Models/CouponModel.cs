using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceStore.Models
{
    public class Coupon
    {
        [Key]
        public int CouponKey { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public couponType coupontype{ get; set; }
        [Required]
        public int Discount { get; set; }
        [Required]
        public int MinAmnt { get; set; }
        public bool isActive { get; set; }
    }
    public enum couponType
    {
        Percent,
        Dollar
    }
}
