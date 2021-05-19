using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceStore.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email Address is Required")]
        public string Email { get; set; }
        
        [Phone]
        [Display(Name ="Phone Number")]
        [Required(ErrorMessage = "Phone Number is Required")]
        public string Phone { get; set; }
        
        [Required(ErrorMessage = "Street Address is required")]
        public string StreetAddress { get; set; }
        
        [Required(ErrorMessage = "City is Required")]
        public string City { get; set; }
        
        [Required(ErrorMessage ="State is required")]
        public string State { get; set; }
        
        [Required(ErrorMessage ="Postal Code is required")]
        public string PostalCode { get; set; }
        
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter a Password")]
        [Compare("ConfirmPassword",ErrorMessage = "Password does not match")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}
