using LibraryManagementSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly INotificationRepository notificationRepo;

        public NotificationsController(INotificationRepository notificationRepo)
        {
            this.notificationRepo = notificationRepo;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "";
            var notifications = notificationRepo.GetByUserId(userId);
            return View("Index", notifications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkAsRead(int id)
        {
            notificationRepo.MarkAsRead(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkAllAsRead()
        {
            var userId = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "";
            notificationRepo.MarkAllAsRead(userId);
            return RedirectToAction("Index");
        }
    }
}