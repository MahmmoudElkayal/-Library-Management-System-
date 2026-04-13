using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Interfaces
{
    public interface IBorrowRepository : IRepository<BorrowRecord>
    {
        List<BorrowRecord> GetAllWithDetails();
        BorrowRecord? GetByIdWithDetails(int id);
        List<BorrowRecord> GetByMemberId(string memberId);
        List<BorrowRecord> GetPendingRequests();
    }
}