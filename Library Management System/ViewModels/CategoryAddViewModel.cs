using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.ViewModels
{
    public class CategoryAddViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string? Description { get; set; }
    }
}