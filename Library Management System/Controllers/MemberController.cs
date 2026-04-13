using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly UserManager<LibraryMember> userManager;
        private readonly SignInManager<LibraryMember> signInManager;
        private readonly IWebHostEnvironment env;

        public MemberController(UserManager<LibraryMember> userManager, SignInManager<LibraryMember> signInManager, IWebHostEnvironment env)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            EditProfileViewModel vm = new EditProfileViewModel
            {
                UserName = user.UserName ?? "",
                Email = user.Email,
                Address = user.Address,
                CurrentProfileImage = user.ProfileImage
            };
            return View("Profile", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(EditProfileViewModel vm)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                if (!string.Equals(user.UserName, vm.UserName, StringComparison.OrdinalIgnoreCase))
                {
                    var setUserName = await userManager.SetUserNameAsync(user, vm.UserName);
                    if (!setUserName.Succeeded)
                    {
                        foreach (var e in setUserName.Errors) ModelState.AddModelError("", e.Description);
                        vm.CurrentProfileImage = user.ProfileImage;
                        return View("Profile", vm);
                    }
                }

                if (!string.IsNullOrWhiteSpace(vm.Email) && !string.Equals(user.Email, vm.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var setEmail = await userManager.SetEmailAsync(user, vm.Email);
                    if (!setEmail.Succeeded)
                    {
                        foreach (var e in setEmail.Errors) ModelState.AddModelError("", e.Description);
                        vm.CurrentProfileImage = user.ProfileImage;
                        return View("Profile", vm);
                    }
                }

                user.Address = vm.Address;

                if (vm.ProfileImage != null && vm.ProfileImage.Length > 0)
                {
                    if (!string.IsNullOrEmpty(user.ProfileImage))
                    {
                        var oldPath = Path.Combine(env.WebRootPath, "images", "members", user.ProfileImage);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    var membersDir = Path.Combine(env.WebRootPath, "images", "members");
                    Directory.CreateDirectory(membersDir);
                    var ext = Path.GetExtension(vm.ProfileImage.FileName);
                    var fileName = $"{Guid.NewGuid():N}{ext}";
                    using (var stream = System.IO.File.Create(Path.Combine(membersDir, fileName)))
                    {
                        await vm.ProfileImage.CopyToAsync(stream);
                    }
                    user.ProfileImage = fileName;
                }

                await userManager.UpdateAsync(user);
                await signInManager.RefreshSignInAsync(user);
                TempData["Success"] = "Profile updated successfully";
                return RedirectToAction(nameof(Profile));
            }

            vm.CurrentProfileImage = user.ProfileImage;
            return View("Profile", vm);
        }
    }
}