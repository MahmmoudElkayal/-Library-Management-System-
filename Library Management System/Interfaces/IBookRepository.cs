using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        List<Book> GetAllWithDetails();
        Book? GetByIdWithDetails(int id);
        List<Book> SearchBooks(string? searchString, int? categoryId, int? authorId);
        List<Book> GetBooksByCategory(int categoryId);
        List<Book> GetAvailableBooks();
        List<Book> GetRecentlyAdded(int count);
    }
}