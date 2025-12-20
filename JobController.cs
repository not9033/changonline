using Microsoft.AspNetCore.Mvc;
using YourProjectName.Models;

namespace YourProjectName.Controllers
{
    public class JobController : Controller
    {
        public IActionResult Index()
        {
            var model = new JobViewModel();
            return View(model);
        }
    }
}