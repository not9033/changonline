using Microsoft.AspNetCore.Mvc;
using ChangOnline.Models;
using ChangOnline.Models.ViewModels;
using ChangOnline.Context;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ChangOnline.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ServiceRequestController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceRequestViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Please login first";
                return RedirectToAction("Index", "AllProvider");
            }

            if (ModelState.IsValid)
            {
                // Fetch location names to store if needed, or rely on IDs.
                // The model assumes storing names as well.
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

                // Handle Image Upload if needed (not implemented in model yet fully but in plan)
                // if (model.ProblemImage != null) { ... }

                _context.ServiceRequests.Add(serviceRequest);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "ส่งคำขอเรียบร้อยแล้ว";
                return RedirectToAction("Index", "AllProvider");
            }

            // If invalid, we should probably show the error. 
            // Since we are posting from a modal in AllProvider, returning View(model) won't work easily 
            // as it expects AllProviderViewModel.
            // For now, redirect back with error (basic handling).
            // If invalid, collect errors with keys
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                .Select(x => $"{x.Key}: {string.Join(", ", x.Value.Errors.Select(e => e.ErrorMessage))}")
                .ToList();
            TempData["ErrorMessage"] = "ข้อมูลไม่ถูกต้อง: " + string.Join(" | ", errors);
            return RedirectToAction("Index", "AllProvider");
        }
    }
}
