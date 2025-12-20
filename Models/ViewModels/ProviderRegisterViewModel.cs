using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ChangOnline.Models.ViewModels
{
    public class ProviderRegisterViewModel
    {
        public IFormFile? ProfileImage { get; set; }
        public string? CurrentImagePath { get; set; }

        [Required(ErrorMessage = "กรุณากรอกข้อมูลบริการ")]
        [Display(Name = "บริการ")]
        public string Service { get; set; }

        [Required(ErrorMessage = "กรุณากรอกที่อยู่")]
        [Display(Name = "ที่อยู่")]
        public string Address { get; set; }

        [Required(ErrorMessage = "กรุณาเลือกจังหวัด")]
        [Display(Name = "จังหวัด")]
        public int? ProvinceId { get; set; }

        [Required(ErrorMessage = "กรุณาเลือกอำเภอ")]
        [Display(Name = "อำเภอ")]
        public int? DistrictId { get; set; }

        [Required(ErrorMessage = "กรุณาเลือกตำบล")]
        [Display(Name = "ตำบล")]
        public int? SubdistrictId { get; set; }

        public List<int> SelectedCategoryIds { get; set; } = new List<int>();
        public IEnumerable<SelectListItem>? Categories { get; set; }

        // For Dropdowns
        public IEnumerable<SelectListItem>? Provinces { get; set; }
        public IEnumerable<SelectListItem>? Districts { get; set; }
        public IEnumerable<SelectListItem>? Subdistricts { get; set; }
    }
}
