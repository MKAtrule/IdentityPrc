using System.ComponentModel.DataAnnotations;

namespace IdentityPrc.ViewModel
{
    public class EditRoleViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required(ErrorMessage = "Role Name is Required")]
        public string RoleName { get; set; }
        [Required(ErrorMessage = "Description  is Required")]
        public string Description { get; set; }
    }
}
