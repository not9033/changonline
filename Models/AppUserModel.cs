using Microsoft.AspNetCore.Identity; // อย่าลืม using

namespace ChangOnline.Models
{
    public class AppUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Phone { get; set; }
        public required string Role { get; set; }
    }
}
