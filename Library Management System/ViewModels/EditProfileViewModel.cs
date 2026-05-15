using System.ComponentModel.DataAnnotations;
using LibraryManagementSystem.Attributes;
using Microsoft.AspNetCore.Http;

namespace LibraryManagementSystem.ViewModels
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string UserName { get; set; } = string.Empty;

        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "Profile Image")]
        [AllowedFileExtensions(ErrorMessage = "Only .jpg, .jpeg, .png, .gif files under 2MB are allowed.")]
        public IFormFile? ProfileImage { get; set; }

        public string? CurrentProfileImage { get; set; }
    }
}