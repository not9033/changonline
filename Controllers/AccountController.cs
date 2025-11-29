using ChangOnline.Models;
using ChangOnline.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChangOnline.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                    Phone = model.Phone,
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
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            // If login fails, we might want to show the modal again with errors.
            // For simplicity in this step, we might redirect to Home with an error or return a view.
            // Since the user wants to use the modal, handling errors gracefully without a full page reload or redirect to a separate login page is tricky without AJAX.
            // However, standard MVC postback will render a view.
            // If we return View(model), we need a Login view. But we are using a modal in Layout.
            // A common pattern for modal login failure in MVC is to redirect to Home but trigger the modal to open and show errors,
            // or return the Home view with model state errors (but Home view expects HomeViewModel usually).
            
            // For now, let's try returning to Home/Index but we need to ensure we can display errors.
            // Or simpler: Redirect to Home with a query parameter indicating error, and JS opens the modal.
            
            // Let's stick to standard behavior first:
            // If failed, we probably should show a dedicated login page OR re-render the current page with the modal open.
            // Given the constraints, I'll return to Home/Index for now, but this might lose ModelState errors if I use Redirect.
            // If I return View("Index", "Home"), I need to pass the HomeViewModel if it exists.
            
            // Let's assume for now we just redirect back to Home with an error flag if possible, or just return a simple error view if it fails.
            // Actually, the user just said "use the login modal".
            // If I return View(), it looks for Login.cshtml.
            
            // Let's implement a basic fallback: if login fails, redirect to Home with an error query param?
            // Or better, let's just return the View which will likely fail if we don't have Login.cshtml.
            
            // I will implement a basic Login.cshtml just in case as a fallback, OR
            // I will change this to redirect to Home with an error message in TempData.
            
            TempData["LoginError"] = "Invalid login attempt.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
