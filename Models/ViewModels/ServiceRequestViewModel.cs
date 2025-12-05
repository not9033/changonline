using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChangOnline.Models.ViewModels
{
    public class ServiceRequestViewModel
    {
        [Required]
        public int ProviderId { get; set; }

        public string? ProviderName { get; set; } // For display in modal

        [Required(ErrorMessage = "กรุณาระบุรายละเอียดงาน")]
        public string JobDetails { get; set; }

        [Required(ErrorMessage = "กรุณาระบุวันที่นัดหมาย")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "กรุณาระบุที่อยู่")]
        public string Address { get; set; }

        [Required(ErrorMessage = "กรุณาเลือกจังหวัด")]
        public int? ProvinceId { get; set; }

        [Required(ErrorMessage = "กรุณาเลือกอำเภอ/เขต")]
        public int? DistrictId { get; set; }

        [Required(ErrorMessage = "กรุณาเลือกตำบล/แขวง")]
        public int? SubdistrictId { get; set; }

        [Required(ErrorMessage = "กรุณาระบุเบอร์โทรศัพท์")]
        [Phone(ErrorMessage = "รูปแบบเบอร์โทรศัพท์ไม่ถูกต้อง")]
        public string ContactNumber { get; set; }

        public IFormFile? ProblemImage { get; set; }

        public string? Remarks { get; set; }

        // Dropdowns
        public List<SelectListItem> Provinces { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Districts { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Subdistricts { get; set; } = new List<SelectListItem>();
    }
}
