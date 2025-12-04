using Microsoft.AspNetCore.Mvc;

namespace ChangOnline.Controllers
{
    public class ProviderController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}
