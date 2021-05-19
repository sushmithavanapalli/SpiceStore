using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using SpiceStore.Data;

namespace SpiceStore.Models
{
    public class SubCategory
    {
        [Key]
        public int SubCategoryKey { get; set; }
        
        [Required(ErrorMessage = "Please enter a valid Sub Category")]
        public string Subcategory { get; set; }

        [Required]
        [Display(Name ="Category")]
        public int CategoryKey { get; set; }

        [ForeignKey("CategoryKey")]
        public virtual Category Categorycode { get; set; }
    }
}
