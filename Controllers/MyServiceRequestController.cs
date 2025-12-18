using Microsoft.AspNetCore.Mvc;
using ChangOnline.Models;
using ChangOnline.Models.ViewModels;
using ChangOnline.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ChangOnline.Controllers
{
    public class MyServiceRequestController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public MyServiceRequestController(
            AppDbContext context,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 👉 เปิดหน้า /MyServiceRequest
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // 👉 POST ส่งคำขอบริการ
        [HttpPost]
        public async Task<IActionResult> Create(ServiceRequestViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "กรุณาเข้าสู่ระบบก่อน";
                return RedirectToAction("Index", "AllProvider");
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "ข้อมูลไม่ครบถ้วน";
                return RedirectToAction("Index", "AllProvider");
            }

            var province = await _context.Provinces.FindAsync(model.ProvinceId);
            var district = await _context.Districts.FindAsync(model.DistrictId);
            var subdistrict = await _context.Subdistricts.FindAsync(model.SubdistrictId);

            var serviceRequest = new ServiceRequest
            {
                RequesterId = user.Id,
                ProviderId = model.ProviderId,
                JobDetails = model.JobDetails,
                AppointmentDate = model.AppointmentDate,
                Address = model.Address,
                Province = province?.NameTh,
                District = district?.NameTh,
                Subdistrict = subdistrict?.name_th,
                ContactNumber = model.ContactNumber,
                Status = ServiceRequestStatus.Pending,
                Remarks = model.Remarks
            };

            _context.ServiceRequests.Add(serviceRequest);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "ส่งคำขอเรียบร้อยแล้ว";
            return RedirectToAction("Index");
        }
    }
}
