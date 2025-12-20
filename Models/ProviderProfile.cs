using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangOnline.Models
{
    public class ProviderProfile
    {
        [Key] public int Id { get; set; }

        [Required] public string UserId { get; set; }

        [ForeignKey("UserId")] public AppUser User { get; set; }

        [Required] public string Service { get; set; }

        [Required] public string Address { get; set; }

        public string? ImagePath { get; set; }

        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? SubdistrictId { get; set; }

        // Navigation properties for location if needed, or just IDs
        // Assuming we might want to display names later
        [ForeignKey("ProvinceId")] public Province? Province { get; set; }

        [ForeignKey("DistrictId")] public District? District { get; set; }

        [ForeignKey("SubdistrictId")] public Subdistrict? Subdistrict { get; set; }

        // Many-to-Many relationship with ProviderCategoryModel
        public ICollection<ProviderCategoryModel> Categories { get; set; } = new List<ProviderCategoryModel>();
    }
}
