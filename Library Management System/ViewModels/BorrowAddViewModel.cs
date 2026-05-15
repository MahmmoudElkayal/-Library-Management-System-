using System.ComponentModel.DataAnnotations;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.ViewModels
{
    public class BorrowAddViewModel
    {
        [Display(Name = "Book")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a book")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Please select a member")]
        [Display(Name = "Member")]
        public string MemberId { get; set; } = string.Empty;

        public List<Book>? Books { get; set; }
        public List<LibraryMember>? Members { get; set; }
    }
}