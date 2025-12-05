using Microsoft.AspNetCore.Identity; // อย่าลืม using

namespace ChangOnline.Models
{
    public class AppUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public required string Role { get; set; }

        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? SubdistrictId { get; set; }
    }
}
