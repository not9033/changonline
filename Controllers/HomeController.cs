using System.Diagnostics;
using ChangOnline.Models;
using ChangOnline.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChangOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(new AllProviderViewModel());
        }

        public IActionResult AllProvider()
        {
            return RedirectToAction("Index", "AllProvider");
        }

        // ✅ แก้ตรงนี้
        public IActionResult MyServiceRequest()
        {
            return RedirectToAction("Index", "MyServiceRequest");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
