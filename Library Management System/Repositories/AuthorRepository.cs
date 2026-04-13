using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext context;

        public AuthorRepository(LibraryDbContext ctx)
        {
            context = ctx;
        }

        public List<Author> GetAll() => context.Authors.ToList();
        public Author? GetById(int id) => context.Authors.FirstOrDefault(a => a.Id == id);
        public void Add(Author entity) => context.Authors.Add(entity);
        public void Update(Author entity) => context.Authors.Update(entity);

        public void Delete(int id)
        {
            Author? author = GetById(id);
            if (author != null)
                context.Authors.Remove(author);
        }

        public void Save() => context.SaveChanges();
    }
}