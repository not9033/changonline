using System.ComponentModel.DataAnnotations;

namespace ChangOnline.Models
{
    public class ProviderCategoryModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }//sub category

        [Required]
        public string Category { get; set; }//main category

        // Navigation property for Many-to-Many relationship
        public ICollection<ProviderProfile> ProviderProfiles { get; set; } = new List<ProviderProfile>();
    }
}
