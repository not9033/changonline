using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using ChangOnline.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ChangOnline.Controllers
{
    public class ProviderController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ChangOnline.Context.AppDbContext _context;

        public ProviderController(UserManager<AppUser> userManager, ChangOnline.Context.AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new Models.ViewModels.ProviderRegisterViewModel();

            // Check if provider profile exists
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

            model.Categories = _context.ProviderCategories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            model.Provinces = _context.Provinces.Select(p => new SelectListItem
            {
                Value = p.ProvinceId.ToString(),
                Text = p.NameTh
            }).ToList();

            if (model.ProvinceId.HasValue)
            {
                model.Districts = _context.Districts.Where(d => d.ProvinceId == model.ProvinceId).Select(d =>
                    new SelectListItem
                    {
                        Value = d.DistrictId.ToString(),
                        Text = d.NameTh
                    }).ToList();
            }

            if (model.DistrictId.HasValue)
            {
                model.Subdistricts = _context.Subdistricts.Where(s => s.district_id == model.DistrictId).Select(s =>
                    new SelectListItem
                    {
                        Value = s.subdistrict_id.ToString(),
                        Text = s.name_th
                    }).ToList();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(Models.ViewModels.ProviderRegisterViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                var providerProfile = _context.ProviderProfiles
                    .Include(p => p.Categories)
                    .FirstOrDefault(p => p.UserId == user.Id);

                if (providerProfile == null)
                {
                    providerProfile = new ProviderProfile
                    {
                        UserId = user.Id
                    };
                    _context.ProviderProfiles.Add(providerProfile);
                }

                providerProfile.Service = model.Service;
                providerProfile.Address = model.Address;
                providerProfile.ProvinceId = model.ProvinceId;
                providerProfile.DistrictId = model.DistrictId;
                providerProfile.SubdistrictId = model.SubdistrictId;

                // Update Categories
                providerProfile.Categories.Clear();
                if (model.SelectedCategoryIds != null && model.SelectedCategoryIds.Any())
                {
                    var selectedCategories = _context.ProviderCategories
                        .Where(c => model.SelectedCategoryIds.Contains(c.Id))
                        .ToList();
                    foreach (var category in selectedCategories)
                    {
                        providerProfile.Categories.Add(category);
                    }
                }

                // Add Provider Role
                if (!await _userManager.IsInRoleAsync(user, "Provider"))
                {
                    await _userManager.AddToRoleAsync(user, "Provider");
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Profile", "Account");
            }

            // Reload dropdowns if failed
            model.Categories = _context.ProviderCategories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            // Reload dropdowns if failed
            model.Provinces = _context.Provinces.Select(p => new SelectListItem
            {
                Value = p.ProvinceId.ToString(),
                Text = p.NameTh
            }).ToList();

            if (model.ProvinceId.HasValue)
            {
                model.Districts = _context.Districts.Where(d => d.ProvinceId == model.ProvinceId).Select(d =>
                    new SelectListItem
                    {
                        Value = d.DistrictId.ToString(),
                        Text = d.NameTh
                    }).ToList();
            }

            if (model.DistrictId.HasValue)
            {
                model.Subdistricts = _context.Subdistricts.Where(s => s.district_id == model.DistrictId).Select(s =>
                    new SelectListItem
                    {
                        Value = s.subdistrict_id.ToString(),
                        Text = s.name_th
                    }).ToList();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult GetDistricts(int provinceId)
        {
            var districts = _context.Districts.Where(d => d.ProvinceId == provinceId)
                .Select(d => new { id = d.DistrictId, name = d.NameTh })
                .ToList();
            return Json(districts);
        }

        [HttpGet]
        public IActionResult GetSubdistricts(int districtId)
        {
            var subdistricts = _context.Subdistricts.Where(s => s.district_id == districtId)
                .Select(s => new { id = s.subdistrict_id, name = s.name_th })
                .ToList();
            return Json(subdistricts);
        }
    }
}
