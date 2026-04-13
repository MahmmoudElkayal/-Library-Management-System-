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
                new Category { Id = 4, Name = "Technology", Description = "IT and programming books" }
            );

            modelBuilder.Entity<Fine>(entity =>
            {
                entity.Property(f => f.Amount).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "Stephen Hawking", Bio = "Theoretical physicist and cosmologist" },
                new Author { Id = 2, Name = "J.K. Rowling", Bio = "British author of Harry Potter series" },
                new Author { Id = 3, Name = "Yuval Noah Harari", Bio = "Israeli historian and professor" },
                new Author { Id = 4, Name = "Andrew Troelsen", Bio = "Software architect and technical author" }
            );
        }
    }
}