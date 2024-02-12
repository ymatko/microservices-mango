using Microsoft.AspNetCore.Identity;

namespace Mango.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string? Role { get; set; }
    }
}
