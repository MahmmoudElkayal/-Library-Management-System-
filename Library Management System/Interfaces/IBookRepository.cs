using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        List<Book> GetAllWithDetails();
        Book? GetByIdWithDetails(int id);
    }
}