using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using ChangOnline.Models;
using ChangOnline.Models.ViewModels;
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
                model.CurrentImagePath = providerProfile.ImagePath;
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

                // Handle image upload
                if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                {
                    // Validate file type
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(model.ProfileImage.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("ProfileImage",
                            "กรุณาอัปโหลดไฟล์รูปภาพเท่านั้น (jpg, jpeg, png, gif)");
                        // Reload dropdowns and return view
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
                        return View(model);
                    }

                    // Validate file size (max 5MB)
                    if (model.ProfileImage.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("ProfileImage", "ขนาดไฟล์ต้องไม่เกิน 5MB");
                        // Reload dropdowns and return view
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
                        return View(model);
                    }

                    // Delete old image if exists (except default)
                    if (!string.IsNullOrEmpty(providerProfile.ImagePath) &&
                        providerProfile.ImagePath != "/image/man.png")
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
                            providerProfile.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Create uploads directory if it doesn't exist
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "providers");
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
                        await model.ProfileImage.CopyToAsync(stream);
                    }

                    // Update image path
                    providerProfile.ImagePath = $"/uploads/providers/{uniqueFileName}";
                }
                else if (string.IsNullOrEmpty(providerProfile.ImagePath))
                {
                    // Set default image if no image uploaded and no existing image
                    providerProfile.ImagePath = "/image/man.png";
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


        [HttpGet]
        public async Task<IActionResult> ProviderInfo(int id)
        {
            var provider = await _context.ProviderProfiles
                .Include(p => p.User)
                .Include(p => p.Categories)
                .Include(p => p.Province)
                .Include(p => p.District)
                .Include(p => p.Subdistrict)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (provider == null)
            {
                return NotFound();
            }

            // Load reviews
            var reviews = await _context.ProviderReviews
                .Include(r => r.Reviewer)
                .Where(r => r.ProviderId == id)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            var viewModel = new ProviderInfoViewModel
            {
                Provider = provider,
                Provinces = await _context.Provinces.Select(p => new SelectListItem
                {
                    Value = p.ProvinceId.ToString(),
                    Text = p.NameTh
                }).ToListAsync(),
                Reviews = reviews,
                AverageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0,
                TotalReviews = reviews.Count
            };

            return View("~/Views/Provider/ProviderInfo.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "กรุณาเข้าสู่ระบบ" });
            }

            var providerProfile = _context.ProviderProfiles.FirstOrDefault(p => p.UserId == user.Id);
            if (providerProfile == null)
            {
                return Json(new { success = false, message = "ไม่พบข้อมูลช่าง" });
            }

            // Delete old image if exists (except default)
            if (!string.IsNullOrEmpty(providerProfile.ImagePath) &&
                providerProfile.ImagePath != "/image/man.png")
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
                    providerProfile.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Reset to default image
            providerProfile.ImagePath = "/image/man.png";
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "ลบรูปภาพสำเร็จ" });
        }

        [HttpGet]
        public async Task<IActionResult> ForProviderPage()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var providerProfile = await _context.ProviderProfiles
                .Include(p => p.User)
                .Include(p => p.Categories)
                .Include(p => p.Province)
                .Include(p => p.District)
                .Include(p => p.Subdistrict)
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (providerProfile == null)
            {
                TempData["ErrorMessage"] = "คุณยังไม่ได้ลงทะเบียนเป็นช่าง";
                return RedirectToAction("Register", "Provider");
            }

            return View(providerProfile);
        }

        [HttpGet]
        public async Task<IActionResult> MyJobs()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Get provider profile
            var providerProfile = await _context.ProviderProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (providerProfile == null)
            {
                TempData["ErrorMessage"] = "คุณยังไม่ได้ลงทะเบียนเป็นช่าง";
                return RedirectToAction("Register", "Provider");
            }

            // Get all service requests for this provider
            var serviceRequests = await _context.ServiceRequests
                .Include(sr => sr.Requester)
                .Where(sr => sr.ProviderId == providerProfile.Id)
                .OrderByDescending(sr => sr.AppointmentDate)
                .ToListAsync();

            return View(serviceRequests);
        }

        public class UpdateJobStatusRequest
        {
            public int RequestId { get; set; }
            public string Status { get; set; } = string.Empty;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateJobStatus([FromBody] UpdateJobStatusRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "กรุณาเข้าสู่ระบบ" });
            }

            var providerProfile = await _context.ProviderProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            if (providerProfile == null)
            {
                return Json(new { success = false, message = "ไม่พบข้อมูลช่าง" });
            }

            var serviceRequest = await _context.ServiceRequests
                .FirstOrDefaultAsync(sr => sr.Id == request.RequestId && sr.ProviderId == providerProfile.Id);

            if (serviceRequest == null)
            {
                return Json(new
                {
                    success = false,
                    message = $"ไม่พบคำขอบริการ (RequestId: {request.RequestId}, ProviderId: {providerProfile.Id})"
                });
            }

            // Update status based on input
            switch (request.Status.ToLower())
            {
                case "accepted":
                    serviceRequest.Status = ServiceRequestStatus.Accepted;
                    break;
                case "rejected":
                    serviceRequest.Status = ServiceRequestStatus.Rejected;
                    break;
                case "completed":
                    serviceRequest.Status = ServiceRequestStatus.Completed;
                    break;
                default:
                    return Json(new { success = false, message = "สถานะไม่ถูกต้อง" });
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "อัปเดตสถานะสำเร็จ" });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitReview(int providerId, int rating, string? comment)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "กรุณาเข้าสู่ระบบ" });
            }

            // Check if provider exists
            var provider = await _context.ProviderProfiles.FindAsync(providerId);
            if (provider == null)
            {
                return Json(new { success = false, message = "ไม่พบข้อมูลช่าง" });
            }

            // Check if user already reviewed this provider
            var existingReview = await _context.ProviderReviews
                .FirstOrDefaultAsync(r => r.ProviderId == providerId && r.ReviewerId == user.Id);

            if (existingReview != null)
            {
                // Update existing review
                existingReview.Rating = rating;
                existingReview.Comment = comment;
                existingReview.CreatedAt = DateTime.Now;
            }
            else
            {
                // Create new review
                var review = new ProviderReview
                {
                    ProviderId = providerId,
                    ReviewerId = user.Id,
                    Rating = rating,
                    Comment = comment,
                    CreatedAt = DateTime.Now
                };
                _context.ProviderReviews.Add(review);
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "ส่งรีวิวสำเร็จ" });
        }
    }
}
