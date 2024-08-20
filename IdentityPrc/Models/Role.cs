using Microsoft.AspNetCore.Identity;

namespace IdentityPrc.Models
{
    public class Role:IdentityRole
    {
        public string Description { get; set; }
    }
}
