using Microsoft.AspNetCore.Http;
using SpiceStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceStore.Models.ViewModels
{
    public class MenuItemViewModel
    {
        [Key]
        public int MenuKey { get; set; }
        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Price must be greater than 1")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Please select a Category")]
        [Display(Name = "Category")]
        public int CategoryKey { get; set; }

        [ForeignKey("CategoryKey")]
        public Category Categorycode { get; set; }

        [Required(ErrorMessage = "Please Select a SubCategory")]
        [Display(Name = "SubCategory")]
        public int SubCategoryKey { get; set; }

        [ForeignKey("SubCategoryKey")]
        public SubCategory SubCategoryCode { get; set; }
        public IFormFile MenuImage { get; set; }
        public spicy spicy { get; set; }
    }
}
