using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpiceStore.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceStore.Models
{
    public class MenuItem
    {
        [Key]
        public int MenuKey { get; set; }
        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Price must be greater than 1")]
        public int Price { get; set; }

        [Display(Name = "Category")]
        public int CategoryKey { get; set; }

        [ForeignKey("CategoryKey")]
        public Category Categorycode { get; set; }

        [Display(Name = "SubCategory")]
        public int SubCategoryKey { get; set; }

        [ForeignKey("SubCategoryKey")]
        public SubCategory SubCategoryCode { get; set; }
        public string MenuImgUrl { get; set; }
        [EnumDataType(typeof(spicy))]
        public spicy spicy { get; set; }
    }
    public enum spicy
    {
        NA = 1,
        NotSpicy = 2,
        Spicy = 3,
        VerySpicy = 4
    }
}
