using ChangOnline.Models;
using ChangOnline.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ChangOnline.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ChangOnline.Context.AppDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ChangOnline.Context.AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var model = new ProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber,
                ProvinceId = user.ProvinceId,
                DistrictId = user.DistrictId,
                SubdistrictId = user.SubdistrictId,
                ImagePath = user.ImagePath,
                IsProvider = await _userManager.IsInRoleAsync(user, "Provider")
            };

            if (model.IsProvider)
            {
                var providerProfile = _context.ProviderProfiles
                    .Include(p => p.Categories)
                    .FirstOrDefault(p => p.UserId == user.Id);

                if (providerProfile != null)
                {
                    model.ProviderAddress = providerProfile.Address;
                    model.ProviderService = providerProfile.Service;
                    model.ProviderProvinceId = providerProfile.ProvinceId;
                    model.ProviderDistrictId = providerProfile.DistrictId;
                    model.ProviderSubdistrictId = providerProfile.SubdistrictId;
                    model.ProviderImagePath = providerProfile.ImagePath;
                    model.ProviderCategoryNames = providerProfile.Categories.Select(c => c.Name).ToList();

                    if (providerProfile.ProvinceId.HasValue)
                        model.ProviderProvinceName = _context.Provinces
                            .FirstOrDefault(p => p.ProvinceId == providerProfile.ProvinceId)?.NameTh;
                    if (providerProfile.DistrictId.HasValue)
                        model.ProviderDistrictName = _context.Districts
                            .FirstOrDefault(d => d.DistrictId == providerProfile.DistrictId)?.NameTh;
                    if (providerProfile.SubdistrictId.HasValue)
                        model.ProviderSubdistrictName = _context.Subdistricts
                            .FirstOrDefault(s => s.subdistrict_id == providerProfile.SubdistrictId)?.name_th;
                }
            }

            if (user.ProvinceId.HasValue)
            {
                model.ProvinceName = _context.Provinces.FirstOrDefault(p => p.ProvinceId == user.ProvinceId)?.NameTh;
            }

            if (user.DistrictId.HasValue)
            {
                model.DistrictName = _context.Districts.FirstOrDefault(d => d.DistrictId == user.DistrictId)?.NameTh;
            }

            if (user.SubdistrictId.HasValue)
            {
                model.SubdistrictName = _context.Subdistricts
                    .FirstOrDefault(s => s.subdistrict_id == user.SubdistrictId)?.name_th;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var model = new ProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber,
                ProvinceId = user.ProvinceId,
                DistrictId = user.DistrictId,
                SubdistrictId = user.SubdistrictId,
                IsProvider = await _userManager.IsInRoleAsync(user, "Provider")
            };

            if (model.IsProvider)
            {
                var providerProfile = _context.ProviderProfiles
                    .Include(p => p.Categories)
                    .FirstOrDefault(p => p.UserId == user.Id);

                if (providerProfile != null)
                {
                    model.ProviderAddress = providerProfile.Address;
                    model.ProviderService = providerProfile.Service;
                    model.ProviderProvinceId = providerProfile.ProvinceId;
                    model.ProviderDistrictId = providerProfile.DistrictId;
                    model.ProviderSubdistrictId = providerProfile.SubdistrictId;
                    model.ProviderCategoryIds = providerProfile.Categories.Select(c => c.Id).ToList();
                }
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

            // Provider Dropdowns
            if (model.IsProvider)
            {
                if (model.ProviderProvinceId.HasValue)
                {
                    model.ProviderDistricts = _context.Districts.Where(d => d.ProvinceId == model.ProviderProvinceId)
                        .Select(d =>
                            new SelectListItem
                            {
                                Value = d.DistrictId.ToString(),
                                Text = d.NameTh
                            }).ToList();
                }

                if (model.ProviderDistrictId.HasValue)
                {
                    model.ProviderSubdistricts = _context.Subdistricts
                        .Where(s => s.district_id == model.ProviderDistrictId).Select(s =>
                            new SelectListItem
                            {
                                Value = s.subdistrict_id.ToString(),
                                Text = s.name_th
                            }).ToList();
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // 1. เช็คความถูกต้องของข้อมูล
            if (ModelState.IsValid)
            {
                // 2. แมพข้อมูลจาก Model ลง User (แก้ไขจุด Phone -> PhoneNumber)
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.Phone; // ✅ ต้องใช้ PhoneNumber (ของ Identity)

                // ข้อมูลที่อยู่
                user.ProvinceId = model.ProvinceId;
                user.DistrictId = model.DistrictId;
                user.SubdistrictId = model.SubdistrictId;

                // 3. สั่งบันทึกลง Database
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // Update Provider Profile if applicable
                    if (await _userManager.IsInRoleAsync(user, "Provider"))
                    {
                        var providerProfile = _context.ProviderProfiles
                            .Include(p => p.Categories)
                            .FirstOrDefault(p => p.UserId == user.Id);

                        if (providerProfile == null)
                        {
                            providerProfile = new ProviderProfile { UserId = user.Id };
                            _context.ProviderProfiles.Add(providerProfile);
                        }

                        providerProfile.Service = model.ProviderService ?? "";
                        providerProfile.Address = model.ProviderAddress ?? "";
                        providerProfile.ProvinceId = model.ProviderProvinceId;
                        providerProfile.DistrictId = model.ProviderDistrictId;
                        providerProfile.SubdistrictId = model.ProviderSubdistrictId;

                        // Update Categories
                        providerProfile.Categories.Clear();
                        if (model.ProviderCategoryIds != null && model.ProviderCategoryIds.Any())
                        {
                            var selectedCategories = _context.ProviderCategories
                                .Where(c => model.ProviderCategoryIds.Contains(c.Id))
                                .ToList();
                            foreach (var category in selectedCategories)
                            {
                                providerProfile.Categories.Add(category);
                            }
                        }

                        await _context.SaveChangesAsync();
                    }

                    TempData["SuccessMessage"] = "บันทึกข้อมูลสำเร็จ!";
                    return RedirectToAction("Profile");
                }

                // ถ้าบันทึกไม่ผ่าน (เช่น เบอร์ซ้ำ หรือกฏ Password)
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                // 🚨 ถ้าไม่เข้า Database ให้ดูตรงนี้! (มันจะปริ้น Error ออกมาที่ Output Console)
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Debug.WriteLine($">>> VALIDATION ERROR: {error.ErrorMessage}");
                }
            }

            // --- 4. โหลด Dropdown กลับมาแสดง (กรณี Error หรือไม่ผ่าน Valid) ---

            model.IsProvider = await _userManager.IsInRoleAsync(user, "Provider");

            model.Categories = _context.ProviderCategories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            // โหลดจังหวัดใหม่เสมอ
            model.Provinces = _context.Provinces.Select(p => new SelectListItem
            {
                Value = p.ProvinceId.ToString(),
                Text = p.NameTh
            }).ToList();

            // ถ้ามีจังหวัดติดมา ให้โหลดอำเภอ
            if (model.ProvinceId.HasValue)
            {
                model.Districts = _context.Districts
                    .Where(d => d.ProvinceId == model.ProvinceId)
                    .Select(d => new SelectListItem
                    {
                        Value = d.DistrictId.ToString(),
                        Text = d.NameTh
                    }).ToList();
            }

            // ถ้ามีอำเภอติดมา ให้โหลดตำบล
            if (model.DistrictId.HasValue)
            {
                model.Subdistricts = _context.Subdistricts
                    .Where(s => s.district_id == model.DistrictId)
                    .Select(s => new SelectListItem
                    {
                        Value = s.subdistrict_id.ToString(),
                        Text = s.name_th
                    }).ToList();
            }

            // Provider Dropdowns
            if (model.IsProvider)
            {
                if (model.ProviderProvinceId.HasValue)
                {
                    model.ProviderDistricts = _context.Districts.Where(d => d.ProvinceId == model.ProviderProvinceId)
                        .Select(d =>
                            new SelectListItem
                            {
                                Value = d.DistrictId.ToString(),
                                Text = d.NameTh
                            }).ToList();
                }

                if (model.ProviderDistrictId.HasValue)
                {
                    model.ProviderSubdistricts = _context.Subdistricts
                        .Where(s => s.district_id == model.ProviderDistrictId).Select(s =>
                            new SelectListItem
                            {
                                Value = s.subdistrict_id.ToString(),
                                Text = s.name_th
                            }).ToList();
                }
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

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.Phone,
                    Role = "User" // Default role
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            TempData["LoginError"] = "Invalid login attempt.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile profileImage)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "กรุณาเข้าสู่ระบบ" });
            }

            if (profileImage == null || profileImage.Length == 0)
            {
                return Json(new { success = false, message = "กรุณาเลือกรูปภาพ" });
            }

            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(profileImage.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return Json(new { success = false, message = "กรุณาอัปโหลดไฟล์รูปภาพเท่านั้น (jpg, jpeg, png, gif)" });
            }

            // Validate file size (max 5MB)
            if (profileImage.Length > 5 * 1024 * 1024)
            {
                return Json(new { success = false, message = "ขนาดไฟล์ต้องไม่เกิน 5MB" });
            }

            // Delete old image if exists (except default)
            if (!string.IsNullOrEmpty(user.ImagePath) && user.ImagePath != "/image/man.png")
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
                    user.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Create uploads directory if it doesn't exist
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");
            if (!Directory.Exists(uploadsDir))
            {
                Directory.CreateDirectory(uploadsDir);
            }

            // Generate unique filename
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsDir, uniqueFileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profileImage.CopyToAsync(stream);
            }

            // Update image path
            user.ImagePath = $"/uploads/profiles/{uniqueFileName}";
            await _userManager.UpdateAsync(user);

            return Json(new { success = true, message = "อัปโหลดรูปภาพสำเร็จ", imagePath = user.ImagePath });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProfileImage()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "กรุณาเข้าสู่ระบบ" });
            }

            // Delete old image if exists (except default)
            if (!string.IsNullOrEmpty(user.ImagePath) && user.ImagePath != "/image/man.png")
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
                    user.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Reset to default image
            user.ImagePath = "/image/man.png";
            await _userManager.UpdateAsync(user);

            return Json(new { success = true, message = "ลบรูปภาพสำเร็จ" });
        }
    }
}
