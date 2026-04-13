using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Interfaces
{
    public interface IFineRepository : IRepository<Fine>
    {
        List<Fine> GetByMemberId(string memberId);
    }
}