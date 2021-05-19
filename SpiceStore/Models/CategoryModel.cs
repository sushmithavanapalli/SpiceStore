using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceStore.Models
{
    public class Category
    {
        [Key]
        public int CategoryKey { get; set; }
        [Display(Name = "Category Name")]
        [Required(ErrorMessage = "Please enter a valid Category")]
        public string CategoryName { get; set; }
    }
}
