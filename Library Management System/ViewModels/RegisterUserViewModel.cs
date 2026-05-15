using System.ComponentModel.DataAnnotations;
using LibraryManagementSystem.Attributes;
using Microsoft.AspNetCore.Http;

namespace LibraryManagementSystem.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? Address { get; set; }

        [Display(Name = "Profile Image")]
        [AllowedFileExtensions(ErrorMessage = "Only .jpg, .jpeg, .png, .gif files under 2MB are allowed.")]
        public IFormFile? ProfileImage { get; set; }
    }
}