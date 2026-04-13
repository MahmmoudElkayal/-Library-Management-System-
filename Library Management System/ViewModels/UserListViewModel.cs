using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.ViewModels
{
    public class UserListViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? ProfileImage { get; set; }
        public string? Address { get; set; }
        public int TotalBorrows { get; set; }
        public int ActiveBorrows { get; set; }
        public int PendingRequests { get; set; }
        public int UnpaidFines { get; set; }
    }
}