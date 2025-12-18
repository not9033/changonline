using System.Collections.Generic;

namespace ChangOnline.Models
{
    public class ProviderProfile
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public string? Service { get; set; }
        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Description { get; set; }

        public int? ProvinceId { get; set; }
        public Province Province { get; set; }

        public int? DistrictId { get; set; }
        public District District { get; set; }

        public int? SubdistrictId { get; set; }
        public Subdistrict Subdistrict { get; set; }

        // ✅ ใช้ชื่อ Model ให้ตรงกับไฟล์จริง
        public ICollection<ProviderCategoryModel> Categories { get; set; }
            = new List<ProviderCategoryModel>();
    }
}
