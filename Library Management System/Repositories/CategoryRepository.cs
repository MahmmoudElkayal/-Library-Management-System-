using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LibraryDbContext context;

        public CategoryRepository(LibraryDbContext ctx)
        {
            context = ctx;
        }

        public List<Category> GetAll() => context.Categories.ToList();
        public Category? GetById(int id) => context.Categories.FirstOrDefault(c => c.Id == id);
        public void Add(Category entity) => context.Categories.Add(entity);
        public void Update(Category entity) => context.Categories.Update(entity);

        public void Delete(int id)
        {
            Category? category = GetById(id);
            if (category != null)
                context.Categories.Remove(category);
        }

        public void Save() => context.SaveChanges();
    }
}