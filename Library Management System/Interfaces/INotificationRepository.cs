using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        List<Notification> GetByUserId(string userId);
        int GetUnreadCount(string userId);
        void MarkAsRead(int id);
        void MarkAllAsRead(string userId);
    }
}