using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.ViewModels
{
    public class UserDetailsViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? ProfileImage { get; set; }
        public string? Address { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public List<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
        public List<Fine> Fines { get; set; } = new List<Fine>();
        public decimal TotalFines { get; set; }
    }
}