using Microsoft.AspNetCore.Identity;

namespace Mango.Web.Models
{
    public class ApplicationUserDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Role { get; set; }
    }
}
