
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReturnFlow.Models;
using ReturnFlow.Models.ViewModels;

namespace ReturnFlow.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(
                    "Email",
                    "An account with this email already exists."
                );

                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Customer");

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                ModelState.AddModelError(
                    string.Empty,
                    "Unable to assign Customer role. Please try again."
                );

                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Dashboard", "Customer");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid email or password."
                );

                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false
            );

            if (!result.Succeeded)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid email or password."
                );

                return View(model);
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("Dashboard", "Admin");

            if (await _userManager.IsInRoleAsync(user, "Manager"))
                return RedirectToAction("Dashboard", "Manager");

            if (await _userManager.IsInRoleAsync(user, "Customer"))
                return RedirectToAction("Dashboard", "Customer");

            await _signInManager.SignOutAsync();

            ModelState.AddModelError(
                string.Empty,
                "No valid role assigned to this account."
            );

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
