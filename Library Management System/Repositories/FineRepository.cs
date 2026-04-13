using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories
{
    public class FineRepository : IFineRepository
    {
        private readonly LibraryDbContext context;

        public FineRepository(LibraryDbContext ctx)
        {
            context = ctx;
        }

        public List<Fine> GetAll() => context.Fines.ToList();

        public List<Fine> GetByMemberId(string memberId) =>
            context.Fines.Include(f => f.BorrowRecord).ThenInclude(f => f.Book).Where(f => f.MemberId == memberId).ToList();

        public Fine? GetById(int id) => context.Fines.FirstOrDefault(f => f.Id == id);
        public void Add(Fine entity) => context.Fines.Add(entity);
        public void Update(Fine entity) => context.Fines.Update(entity);

        public void Delete(int id)
        {
            Fine? fine = GetById(id);
            if (fine != null)
                context.Fines.Remove(fine);
        }

        public void Save() => context.SaveChanges();
    }
}