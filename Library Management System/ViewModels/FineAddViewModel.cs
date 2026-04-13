using System.ComponentModel.DataAnnotations;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.ViewModels
{
    public class FineAddViewModel
    {
        [Required(ErrorMessage = "Please select a member")]
        [Display(Name = "Member")]
        public string MemberId { get; set; }

        [Required(ErrorMessage = "Please select a borrow record")]
        [Display(Name = "Borrow Record")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a borrow record")]
        public int BorrowRecordId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        public List<LibraryMember>? Members { get; set; }
        public List<BorrowRecord>? BorrowRecords { get; set; }
    }
}