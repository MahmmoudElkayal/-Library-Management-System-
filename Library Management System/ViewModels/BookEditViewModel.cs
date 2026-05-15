using System.ComponentModel.DataAnnotations;
using LibraryManagementSystem.Attributes;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Http;

namespace LibraryManagementSystem.ViewModels
{
    public class BookEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        public string? ISBN { get; set; }

        [Display(Name = "Cover Image")]
        [AllowedFileExtensions(ErrorMessage = "Only .jpg, .jpeg, .png, .gif files under 2MB are allowed.")]
        public IFormFile? CoverImage { get; set; }

        public string? CurrentCoverImage { get; set; }

        [Required(ErrorMessage = "Author is required")]
        [Display(Name = "Author")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }

        public List<Author>? Authors { get; set; }
        public List<Category>? Categories { get; set; }
    }
}