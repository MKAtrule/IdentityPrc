using System.ComponentModel.DataAnnotations;

namespace IdentityPrc.ViewModel
{
    public class EditUserViewModel
    {
        
        [Required]
        public string Id { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Bio")]
        public string Bio { get; set; }
        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }
    }
}
