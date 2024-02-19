using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Mango.Web.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ApplicationUserDto
    {
        [JsonProperty(Order = 1)]
        public string UserId { get; set; }

        [JsonProperty(Order = 2)]
        public string Name { get; set; }

        [JsonProperty(Order = 3)]
        public string Email { get; set; }

        [JsonProperty(Order = 4)]
        public string PhoneNumber { get; set; }

        [JsonProperty(Order = 5)]
        public string? Role { get; set; }
    }
}
