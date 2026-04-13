using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Models
{
    public class LibraryDbContext : IdentityDbContext<LibraryMember>
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }
        public DbSet<Fine> Fines { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BorrowRecord>()
                .HasOne(br => br.Book)
                .WithMany(b => b.BorrowRecords)
                .HasForeignKey(br => br.BookId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BorrowRecord>()
                .HasOne(br => br.Member)
                .WithMany(m => m.BorrowRecords)
                .HasForeignKey(br => br.MemberId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Fine>()
                .HasOne(f => f.BorrowRecord)
                .WithMany(br => br.Fines)
                .HasForeignKey(f => f.BorrowRecordId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Fine>()
                .HasOne(f => f.Member)
                .WithMany()
                .HasForeignKey(f => f.MemberId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Science", Description = "Science books and textbooks" },
                new Category { Id = 2, Name = "Fiction", Description = "Novels and fiction literature" },
                new Category { Id = 3, Name = "History", Description = "History books and biographies" },
                new Category { Id = 4, Name = "Technology", Description = "IT and programming books" },
                new Category { Id = 5, Name = "Literature", Description = "Classic and contemporary world literature" }
            );

            modelBuilder.Entity<Fine>(entity =>
            {
                entity.Property(f => f.Amount).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "Stephen Hawking", Bio = "Theoretical physicist and cosmologist" },
                new Author { Id = 2, Name = "J.K. Rowling", Bio = "British author of Harry Potter series" },
                new Author { Id = 3, Name = "Yuval Noah Harari", Bio = "Israeli historian and professor" },
                new Author { Id = 4, Name = "Andrew Troelsen", Bio = "Software architect and technical author" },
                new Author { Id = 5, Name = "F. Scott Fitzgerald", Bio = "American novelist of the Jazz Age" },
                new Author { Id = 6, Name = "Harper Lee", Bio = "American novelist best known for To Kill a Mockingbird" },
                new Author { Id = 7, Name = "Kahlil Gibran", Bio = "Lebanese-American writer, poet and visual artist" },
                new Author { Id = 8, Name = "Khaled Hosseini", Bio = "Afghan-American novelist and physician" },
                new Author { Id = 9, Name = "الطيب صالح (Tayeb Salih)", Bio = "Sudanese novelist and journalist" },
                new Author { Id = 10, Name = "غسان كنفاني (Ghassan Kanafani)", Bio = "Palestinian writer and political activist" },
                new Author { Id = 11, Name = "نجيب محفوظ (Naguib Mahfouz)", Bio = "Egyptian novelist, Nobel Prize laureate in Literature 1988" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "A Brief History of Time", ISBN = "978-0553380163", CoverImage = "brief-history.jpg", AuthorId = 1, CategoryId = 1 },
                new Book { Id = 2, Title = "The Universe in a Nutshell", ISBN = "978-0553802023", CoverImage = "universe-nutshell.jpg", AuthorId = 1, CategoryId = 1 },
                new Book { Id = 3, Title = "Harry Potter and the Sorcerer's Stone", ISBN = "978-0590353427", CoverImage = "hp-stone.jpg", AuthorId = 2, CategoryId = 2 },
                new Book { Id = 4, Title = "Harry Potter and the Chamber of Secrets", ISBN = "978-0439064873", CoverImage = "hp-chamber.jpg", AuthorId = 2, CategoryId = 2 },
                new Book { Id = 5, Title = "Harry Potter and the Prisoner of Azkaban", ISBN = "978-0439136365", CoverImage = "hp-azkaban.jpg", AuthorId = 2, CategoryId = 2 },
                new Book { Id = 6, Title = "Sapiens: A Brief History of Humankind", ISBN = "978-0062316097", CoverImage = "sapiens.jpg", AuthorId = 3, CategoryId = 3 },
                new Book { Id = 7, Title = "Homo Deus: A Brief History of Tomorrow", ISBN = "978-0062464316", CoverImage = "homo-deus.jpg", AuthorId = 3, CategoryId = 3 },
                new Book { Id = 8, Title = "21 Lessons for the 21st Century", ISBN = "978-0525512172", CoverImage = "21-lessons.jpg", AuthorId = 3, CategoryId = 3 },
                new Book { Id = 9, Title = "Pro C# 10 with .NET 6", ISBN = "978-1484273616", CoverImage = "pro-csharp.jpg", AuthorId = 4, CategoryId = 4 },
                new Book { Id = 10, Title = "C# and the .NET Platform", ISBN = "978-1590590558", CoverImage = "dotnet-platform.jpg", AuthorId = 4, CategoryId = 4 },
                new Book { Id = 11, Title = "The Great Gatsby", ISBN = "978-0743273565", CoverImage = "great-gatsby.jpg", AuthorId = 5, CategoryId = 2 },
                new Book { Id = 12, Title = "To Kill a Mockingbird", ISBN = "978-0061120084", CoverImage = "mockingbird.jpg", AuthorId = 6, CategoryId = 2 },
                new Book { Id = 13, Title = "The Prophet", ISBN = "978-0394404288", CoverImage = "prophet.jpg", AuthorId = 7, CategoryId = 5 },
                new Book { Id = 14, Title = "The Kite Runner", ISBN = "978-1594631931", CoverImage = "kite-runner.jpg", AuthorId = 8, CategoryId = 2 },
                new Book { Id = 15, Title = "موسم الهجرة إلى الشمال", ISBN = "978-0141190600", CoverImage = "season-migration.jpg", AuthorId = 9, CategoryId = 2 },
                new Book { Id = 16, Title = "رجال في الشمس", ISBN = "978-0894107627", CoverImage = "men-in-sun.jpg", AuthorId = 10, CategoryId = 2 },
                new Book { Id = 17, Title = "زقاق المدق", ISBN = "978-9774160615", CoverImage = "midaq-alley.jpg", AuthorId = 11, CategoryId = 2 }
            );
        }
    }
}