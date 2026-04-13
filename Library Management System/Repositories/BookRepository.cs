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

        public List<Book> SearchBooks(string? searchString, int? categoryId, int? authorId)
        {
            var query = context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(b => 
                    b.Title.ToLower().Contains(searchString) ||
                    (b.Author != null && b.Author.Name.ToLower().Contains(searchString)) ||
                    (b.Category != null && b.Category.Name.ToLower().Contains(searchString)) ||
                    (b.ISBN != null && b.ISBN.Contains(searchString)));
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(b => b.CategoryId == categoryId.Value);
            }

            if (authorId.HasValue && authorId.Value > 0)
            {
                query = query.Where(b => b.AuthorId == authorId.Value);
            }

            return query.ToList();
        }

        public List<Book> GetBooksByCategory(int categoryId)
        {
            return context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => b.CategoryId == categoryId)
                .ToList();
        }

        public List<Book> GetAvailableBooks()
        {
            var borrowedBookIds = context.BorrowRecords
                .Where(br => br.ReturnDate == null)
                .Select(br => br.BookId)
                .ToList();

            return context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => !borrowedBookIds.Contains(b.Id))
                .ToList();
        }

        public List<Book> GetRecentlyAdded(int count)
        {
            return context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .OrderByDescending(b => b.Id)
                .Take(count)
                .ToList();
        }

        public string GetAvailabilityStatus(int bookId)
        {
            var activeBorrow = context.BorrowRecords
                .FirstOrDefault(br => br.BookId == bookId && (br.Status == "Borrowed" || br.Status == "Pending"));
            if (activeBorrow != null)
            {
                return activeBorrow.Status;
            }
            return "Available";
        }

        public Dictionary<int, string> GetAvailabilityStatuses()
        {
            var statuses = new Dictionary<int, string>();
            var activeBorrows = context.BorrowRecords
                .Where(br => br.Status == "Borrowed" || br.Status == "Pending" || br.Status == "Overdue")
                .ToList();

            foreach (var book in context.Books)
            {
                var borrow = activeBorrows.FirstOrDefault(br => br.BookId == book.Id);
                if (borrow != null)
                {
                    statuses[book.Id] = borrow.Status;
                }
                else
                {
                    statuses[book.Id] = "Available";
                }
            }

            return statuses;
        }
    }
}