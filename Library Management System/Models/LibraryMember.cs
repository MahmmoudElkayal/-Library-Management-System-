using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem.Models
{
    public class LibraryMember : IdentityUser
    {
        public string? Address { get; set; }
        public string? ProfileImage { get; set; }
        public ICollection<BorrowRecord>? BorrowRecords { get; set; }
    }
}