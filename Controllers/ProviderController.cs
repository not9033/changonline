using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using ChangOnline.Models;
using Microsoft.EntityFrameworkCore;

namespace ChangOnline.Controllers
{
    public class ProviderController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ChangOnline.Context.AppDbContext _context;

        public ProviderController(
            UserManager<AppUser> userManager,
            ChangOnline.Context.AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // ================= สมัครเป็นช่าง =================
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var model = new Models.ViewModels.ProviderRegisterViewModel();

            var providerProfile = _context.ProviderProfiles
                .Include(p => p.Categories)
                .FirstOrDefault(p => p.UserId == user.Id);

            if (providerProfile != null)
            {
                model.Address = providerProfile.Address;
                model.Service = providerProfile.Service;
                model.ProvinceId = providerProfile.ProvinceId;
                model.DistrictId = providerProfile.DistrictId;
                model.SubdistrictId = providerProfile.SubdistrictId;
                model.SelectedCategoryIds = providerProfile.Categories.Select(c => c.Id).ToList();
            }

            model.Categories = _context.ProviderCategories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            model.Provinces = _context.Provinces
                .Select(p => new SelectListItem
                {
                    Value = p.ProvinceId.ToString(),
                    Text = p.NameTh
                }).ToList();

            return View(model);
        }

        // ================= ดูโปรไฟล์ช่าง =================
        [HttpGet]
        public async Task<IActionResult> ProviderInfo(int? id)
        {
            ProviderProfile provider;

            // if (id.HasValue)
            // {
            //     // ดูช่างคนอื่น
            //     provider = await _context.ProviderProfiles
            //         .Include(p => p.User)
            //         .Include(p => p.Categories)
            //         .Include(p => p.Province)
            //         .Include(p => p.District)
            //         .Include(p => p.Subdistrict)
            //         .FirstOrDefaultAsync(p => p.Id == id.Value);
            // }
            // else
            // {
            //     // ดูโปรไฟล์ตัวเอง
            //     var user = await _userManager.GetUserAsync(User);
            //     if (user == null)
            //         return RedirectToAction("Login", "Account");

            //     provider = await _context.ProviderProfiles
            //         .Include(p => p.User)
            //         .Include(p => p.Categories)
            //         .Include(p => p.Province)
            //         .Include(p => p.District)
            //         .Include(p => p.Subdistrict)
            //         .FirstOrDefaultAsync(p => p.UserId == user.Id);
            // }

            // if (provider == null)
            //     return NotFound();

            return View("~/Views/Provider/ProviderInfo.cshtml" );
        }

        // ================= Ajax =================
        [HttpGet]
        public IActionResult GetDistricts(int provinceId)
        {
            var districts = _context.Districts
                .Where(d => d.ProvinceId == provinceId)
                .Select(d => new { id = d.DistrictId, name = d.NameTh })
                .ToList();

            return Json(districts);
        }

        [HttpGet]
        public IActionResult GetSubdistricts(int districtId)
        {
            var subs = _context.Subdistricts
                .Where(s => s.district_id == districtId)
                .Select(s => new { id = s.subdistrict_id, name = s.name_th })
                .ToList();

            return Json(subs);
        }
    }
}
