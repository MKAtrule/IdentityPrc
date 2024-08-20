using Microsoft.AspNetCore.Identity;

namespace IdentityPrc.Models
{
    public class User:IdentityUser
    {
        public string Bio { get; set; }
    }
}
