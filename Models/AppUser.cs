using Microsoft.AspNetCore.Identity;

namespace ChangOnline.Models
{
    // สืบทอดจาก IdentityUser เพื่อใช้กับ IdentityDbContext
    public class AppUser : IdentityUser
    {
        // ชื่อจริง-นามสกุล
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // เบอร์โทร
        public string Phone { get; set; } = string.Empty;

        // Role ที่ใช้ใน MockData/AdminController
        public string Role { get; set; } = string.Empty;
    }
}
