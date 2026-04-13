using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly LibraryDbContext context;

        public NotificationRepository(LibraryDbContext ctx)
        {
            context = ctx;
        }

        public List<Notification> GetAll() => context.Notifications.OrderByDescending(n => n.CreatedAt).ToList();

        public Notification? GetById(int id) => context.Notifications.FirstOrDefault(n => n.Id == id);

        public List<Notification> GetByUserId(string userId) =>
            context.Notifications.Where(n => n.UserId == userId).OrderByDescending(n => n.CreatedAt).ToList();

        public int GetUnreadCount(string userId) =>
            context.Notifications.Count(n => n.UserId == userId && !n.IsRead);

        public void Add(Notification entity) => context.Notifications.Add(entity);
        public void Update(Notification entity) => context.Notifications.Update(entity);

        public void Delete(int id)
        {
            Notification? notification = GetById(id);
            if (notification != null)
                context.Notifications.Remove(notification);
        }

        public void Save() => context.SaveChanges();

        public void MarkAsRead(int id)
        {
            var notification = GetById(id);
            if (notification != null)
            {
                notification.IsRead = true;
                context.Notifications.Update(notification);
                context.SaveChanges();
            }
        }

        public void MarkAllAsRead(string userId)
        {
            var unread = context.Notifications.Where(n => n.UserId == userId && !n.IsRead).ToList();
            foreach (var notification in unread)
            {
                notification.IsRead = true;
            }
            context.Notifications.UpdateRange(unread);
            context.SaveChanges();
        }
    }
}