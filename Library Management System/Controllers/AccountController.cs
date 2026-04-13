using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<LibraryMember> userManager;
        private readonly SignInManager<LibraryMember> signInManager;

        public AccountController(UserManager<LibraryMember> userManager, SignInManager<LibraryMember> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var identifier = (vm.Identifier ?? "").Trim();
                LibraryMember? user = await userManager.FindByNameAsync(identifier);
                if (user == null && identifier.Contains("@"))
                {
                    user = await userManager.FindByEmailAsync(identifier);
                }

                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl))
                        {
                            return Redirect(vm.ReturnUrl);
                        }

                        if (await userManager.IsInRoleAsync(user, "Admin"))
                        {
                            return RedirectToAction("Dashboard", "Home");
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Invalid username/password.");
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel vm)
        {
            if (ModelState.IsValid)
            {
                LibraryMember appUser = new LibraryMember
                {
                    UserName = vm.UserName,
                    Email = vm.Email,
                    Address = vm.Address
                };

                if (vm.ProfileImage != null && vm.ProfileImage.Length > 0)
                {
                    var membersDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "members");
                    Directory.CreateDirectory(membersDir);

                    var ext = Path.GetExtension(vm.ProfileImage.FileName);
                    var fileName = $"{Guid.NewGuid():N}{ext}";
                    var physicalPath = Path.Combine(membersDir, fileName);
                    using (var stream = System.IO.File.Create(physicalPath))
                    {
                        await vm.ProfileImage.CopyToAsync(stream);
                    }
                    appUser.ProfileImage = fileName;
                }

                IdentityResult result = await userManager.CreateAsync(appUser, vm.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(appUser, "Member");
                    await signInManager.SignInAsync(appUser, false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View("Register", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}