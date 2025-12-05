using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ChangOnline.Models.ViewModels
{
    public class ProfileViewModel
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }

        [Display(Name = "Province")] public int? ProvinceId { get; set; }

        [Display(Name = "District")] public int? DistrictId { get; set; }

        [Display(Name = "Subdistrict")] public int? SubdistrictId { get; set; }

        // Provider Specific
        public bool IsProvider { get; set; }

        [Display(Name = "Service")] public string? ProviderService { get; set; }
        [Display(Name = "Shop Address")] public string? ProviderAddress { get; set; }

        public int? ProviderProvinceId { get; set; }
        public int? ProviderDistrictId { get; set; }
        public int? ProviderSubdistrictId { get; set; }

        public string? ProvinceName { get; set; }
        public string? DistrictName { get; set; }
        public string? SubdistrictName { get; set; }

        public string? ProviderProvinceName { get; set; }
        public string? ProviderDistrictName { get; set; }
        public string? ProviderSubdistrictName { get; set; }

        public List<int> ProviderCategoryIds { get; set; } = new List<int>();
        public List<string> ProviderCategoryNames { get; set; } = new List<string>();
        public IEnumerable<SelectListItem>? Categories { get; set; }

        public IEnumerable<SelectListItem>? Provinces { get; set; }
        public IEnumerable<SelectListItem>? Districts { get; set; }
        public IEnumerable<SelectListItem>? Subdistricts { get; set; }

        public IEnumerable<SelectListItem>? ProviderDistricts { get; set; }
        public IEnumerable<SelectListItem>? ProviderSubdistricts { get; set; }
    }
}
