using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<LibraryMember> userManager;
        private readonly IBorrowRepository borrowRepo;
        private readonly IFineRepository fineRepo;

        public UsersController(UserManager<LibraryMember> userManager, IBorrowRepository borrowRepo, IFineRepository fineRepo)
        {
            this.userManager = userManager;
            this.borrowRepo = borrowRepo;
            this.fineRepo = fineRepo;
        }

        public IActionResult Index()
        {
            var members = userManager.Users.ToList();
            var viewModel = members.Select(m => new UserListViewModel
            {
                Id = m.Id,
                UserName = m.UserName ?? "",
                Email = m.Email ?? "",
                ProfileImage = m.ProfileImage,
                Address = m.Address,
                TotalBorrows = borrowRepo.GetByMemberId(m.Id).Count,
                ActiveBorrows = borrowRepo.GetByMemberId(m.Id).Count(br => br.Status == "Borrowed" || br.Status == "Overdue"),
                PendingRequests = borrowRepo.GetByMemberId(m.Id).Count(br => br.Status == "Pending"),
                UnpaidFines = fineRepo.GetByMemberId(m.Id).Count(f => !f.IsPaid)
            }).ToList();

            return View("Index", viewModel);
        }

        public async Task<IActionResult> Details(string id)
        {
            var member = await userManager.FindByIdAsync(id);
            if (member == null) return NotFound();

            var borrowRecords = borrowRepo.GetByMemberId(id);
            var fines = fineRepo.GetByMemberId(id);
            var roles = await userManager.GetRolesAsync(member);

            var viewModel = new UserDetailsViewModel
            {
                Id = member.Id,
                UserName = member.UserName ?? "",
                Email = member.Email ?? "",
                ProfileImage = member.ProfileImage,
                Address = member.Address,
                Roles = roles.ToList(),
                BorrowRecords = borrowRecords,
                Fines = fines,
                TotalFines = fines.Where(f => !f.IsPaid).Sum(f => f.Amount)
            };

            return View("Details", viewModel);
        }
    }
}