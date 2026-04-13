using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FinesController : Controller
    {
        private readonly IFineRepository fineRepo;
        private readonly IBorrowRepository borrowRepo;
        private readonly UserManager<LibraryMember> userManager;

        public FinesController(IFineRepository fineRepo, IBorrowRepository borrowRepo, UserManager<LibraryMember> userManager)
        {
            this.fineRepo = fineRepo;
            this.borrowRepo = borrowRepo;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            List<Fine> fines = fineRepo.GetAll();
            return View("Index", fines);
        }

        [HttpGet]
        public IActionResult Create()
        {
            FineAddViewModel vm = new FineAddViewModel
            {
                Members = userManager.Users.ToList(),
                BorrowRecords = borrowRepo.GetAll()
            };
            return View("Create", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FineAddViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Fine fine = new Fine
                    {
                        Amount = vm.Amount,
                        IsPaid = false,
                        BorrowRecordId = vm.BorrowRecordId,
                        MemberId = vm.MemberId
                    };
                    fineRepo.Add(fine);
                    fineRepo.Save();
                    TempData["Success"] = "Fine created successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
                }
            }

            vm.Members = userManager.Users.ToList();
            vm.BorrowRecords = borrowRepo.GetAll();
            return View("Create", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkPaid(int id)
        {
            Fine? fine = fineRepo.GetById(id);
            if (fine != null)
            {
                fine.IsPaid = true;
                fineRepo.Update(fine);
                fineRepo.Save();
                TempData["Success"] = "Fine marked as paid";
            }
            return RedirectToAction("Index");
        }
    }
}