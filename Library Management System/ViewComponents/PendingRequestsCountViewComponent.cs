using LibraryManagementSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.ViewComponents
{
    public class PendingRequestsCountViewComponent : ViewComponent
    {
        private readonly IBorrowRepository borrowRepo;

        public PendingRequestsCountViewComponent(IBorrowRepository borrowRepo)
        {
            this.borrowRepo = borrowRepo;
        }

        public IViewComponentResult Invoke()
        {
            int count = borrowRepo.GetPendingRequests().Count;
            return View(count);
        }
    }
}