using System.Diagnostics;
using ChangOnline.Models;
using ChangOnline.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ChangOnline.Context;

namespace ChangOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Load random reviews (up to 6)
            var randomReviews = await _context.ProviderReviews
                .Include(r => r.Reviewer)
                .Include(r => r.Provider)
                .Where(r => !string.IsNullOrEmpty(r.Comment))
                .OrderBy(r => Guid.NewGuid())
                .Take(6)
                .ToListAsync();

            var viewModel = new AllProviderViewModel
            {
                RandomReviews = randomReviews
            };

            return View(viewModel);
        }

        public IActionResult AllProvider()
        {
            return RedirectToAction("Index", "AllProvider");
        }

        public async Task<IActionResult> MyServiceRequest()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var serviceRequests = await _context.ServiceRequests
                .Include(sr => sr.Provider)
                .ThenInclude(p => p.User)
                .Where(sr => sr.RequesterId == user.Id)
                .OrderByDescending(sr => sr.AppointmentDate)
                .ToListAsync();

            return View("MyServiceRequest", serviceRequests);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
