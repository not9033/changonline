using ChangOnline.Context;
using ChangOnline.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ChangOnline.Controllers
{
    public class AllProviderController : Controller
    {
        private readonly AppDbContext _context;

        public AllProviderController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? searchTerm, string? category, int page = 1)
        {
            int pageSize = 8; // จำนวนที่เเสดงต่อหน้า

            var query = _context.ProviderProfiles
                .Include(p => p.User)
                .Include(p => p.Categories) // Include Categories for filtering
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p =>
                    p.User.FirstName.Contains(searchTerm) ||
                    p.User.LastName.Contains(searchTerm) ||
                    p.Service.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Categories.Any(c => c.Name == category));
            }

            int totalCount = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // Ensure page is within valid range
            page = Math.Max(1, Math.Min(page, totalPages > 0 ? totalPages : 1));

            var providers = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new AllProviderViewModel
            {
                Providers = providers,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalCount = totalCount,
                SearchTerm = searchTerm,
                Category = category,
                Provinces = await _context.Provinces.Select(p => new SelectListItem
                {
                    Value = p.ProvinceId.ToString(),
                    Text = p.NameTh
                }).ToListAsync()
            };

            return View("~/Views/Home/AllProvider.cshtml", viewModel);
            // Explicitly pointing to the existing view location if it's not in Views/AllProvider/
            // Based on user context, it was in Views/Home/AllProvider.cshtml. 
            // If I move the view, I can change this. For now, I'll point to it.
            // Actually, usually controllers have their own folder. 
            // But let's check if I should move the view or just point to it.
            // User context said: Active Document: ...\Views\Home\AllProvider.cshtml
            // So it is currently in Home. 
            // If I return View(viewModel) from AllProviderController, it looks in Views/AllProvider/Index.cshtml or Views/Shared/...
            // I should probably move the view or return the specific path. 
            // Returning specific path is safer for now to avoid breaking other things if Home uses it.
            // But wait, the user wants "AllProviderController" to show "Provider to AllProvider page".
            // Ideally, I should move Views/Home/AllProvider.cshtml to Views/AllProvider/Index.cshtml
            // BUT, the user might be linking to it as /Home/AllProvider currently. 
            // I will return View("~/Views/Home/AllProvider.cshtml", viewModel); as a safe bet.
        }
    }
}
