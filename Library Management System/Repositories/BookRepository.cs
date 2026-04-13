using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext context;

        public BookRepository(LibraryDbContext ctx)
        {
            context = ctx;
        }

        public List<Book> GetAll() => context.Books.ToList();

        public List<Book> GetAllWithDetails() =>
            context.Books.Include(b => b.Author).Include(b => b.Category).ToList();

        public Book? GetById(int id) => context.Books.FirstOrDefault(b => b.Id == id);

        public Book? GetByIdWithDetails(int id) =>
            context.Books.Include(b => b.Author).Include(b => b.Category).FirstOrDefault(b => b.Id == id);

        public void Add(Book entity) => context.Books.Add(entity);
        public void Update(Book entity) => context.Books.Update(entity);

        public void Delete(int id)
        {
            Book? book = GetById(id);
            if (book != null)
                context.Books.Remove(book);
        }

        public void Save() => context.SaveChanges();
    }
}