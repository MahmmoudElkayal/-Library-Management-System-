using System.ComponentModel.DataAnnotations;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.ViewModels
{
    public class BorrowRequestViewModel
    {
        [Display(Name = "Book")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a book")]
        public int BookId { get; set; }

        public List<Book>? Books { get; set; }
    }
}