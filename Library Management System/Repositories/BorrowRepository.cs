using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly LibraryDbContext context;

        public BorrowRepository(LibraryDbContext ctx)
        {
            context = ctx;
        }

        public List<BorrowRecord> GetAll() => context.BorrowRecords.ToList();

        public List<BorrowRecord> GetAllWithDetails() =>
            context.BorrowRecords.Include(br => br.Book).Include(br => br.Member).ToList();

        public BorrowRecord? GetById(int id) => context.BorrowRecords.FirstOrDefault(br => br.Id == id);

        public BorrowRecord? GetByIdWithDetails(int id) =>
            context.BorrowRecords.Include(br => br.Book).Include(br => br.Member).FirstOrDefault(br => br.Id == id);

        public List<BorrowRecord> GetByMemberId(string memberId) =>
            context.BorrowRecords.Include(br => br.Book).Where(br => br.MemberId == memberId).ToList();

        public List<BorrowRecord> GetPendingRequests() =>
            context.BorrowRecords.Include(br => br.Book).Include(br => br.Member)
                .Where(br => br.Status == "Pending").ToList();

        public void Add(BorrowRecord entity) => context.BorrowRecords.Add(entity);
        public void Update(BorrowRecord entity) => context.BorrowRecords.Update(entity);

        public void Delete(int id)
        {
            BorrowRecord? record = GetById(id);
            if (record != null)
                context.BorrowRecords.Remove(record);
        }

        public void Save() => context.SaveChanges();
    }
}