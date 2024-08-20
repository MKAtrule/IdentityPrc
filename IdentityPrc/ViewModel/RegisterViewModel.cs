using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IdentityPrc.ViewModel
{
    
        public class RegisterViewModel
        {
             [Required]
            [MaxLength(100)]
             public string Bio {  get; set; } 
            [Required]
            [EmailAddress]
            [Remote(action: "IsEmailAvailable", controller: "Account")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
    
}
