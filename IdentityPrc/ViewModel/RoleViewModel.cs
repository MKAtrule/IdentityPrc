using System.ComponentModel.DataAnnotations;

namespace IdentityPrc.ViewModel
{
    public class RoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
        [Required]
        [Display(Name = "Description")]

        public string Description { get; set; }
    }
}
