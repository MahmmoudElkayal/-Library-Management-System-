using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class BorrowController : Controller
    {
        private readonly IBorrowRepository borrowRepo;
        private readonly IBookRepository bookRepo;
        private readonly IFineRepository fineRepo;
        private readonly UserManager<LibraryMember> userManager;

        public BorrowController(IBorrowRepository borrowRepo, IBookRepository bookRepo, IFineRepository fineRepo, UserManager<LibraryMember> userManager)
        {
            this.borrowRepo = borrowRepo;
            this.bookRepo = bookRepo;
            this.fineRepo = fineRepo;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            List<BorrowRecord> records = borrowRepo.GetAllWithDetails();
            return View("Index", records);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult PendingRequests()
        {
            List<BorrowRecord> pending = borrowRepo.GetPendingRequests();
            return View("PendingRequests", pending);
        }

        [Authorize(Roles = "Member")]
        public IActionResult MyBorrows()
        {
            var memberId = userManager.GetUserId(User) ?? "";
            List<BorrowRecord> records = borrowRepo.GetByMemberId(memberId);
            return View("MyBorrows", records);
        }

        [HttpGet]
        [Authorize(Roles = "Member")]
        public IActionResult RequestBook()
        {
            BorrowRequestViewModel vm = new BorrowRequestViewModel
            {
                Books = bookRepo.GetAll()
            };
            return View("RequestBook", vm);
        }

        [HttpPost]
        [Authorize(Roles = "Member")]
        [ValidateAntiForgeryToken]
        public IActionResult RequestBook(BorrowRequestViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var memberId = userManager.GetUserId(User) ?? "";

                    var existingRecord = borrowRepo.GetAll()
                        .FirstOrDefault(br => br.BookId == vm.BookId && br.MemberId == memberId
                            && (br.Status == BorrowStatus.Pending || br.Status == BorrowStatus.Borrowed));
                    if (existingRecord != null)
                    {
                        ModelState.AddModelError("", "You already have a pending or active borrow for this book.");
                    }
                    else
                    {
                        BorrowRecord record = new BorrowRecord
                        {
                            BookId = vm.BookId,
                            MemberId = memberId,
                            RequestedDate = DateTime.Now,
                            BorrowDate = null,
                            ReturnDate = null,
                            Status = BorrowStatus.Pending
                        };
                        borrowRepo.Add(record);
                        borrowRepo.Save();
                        TempData["Success"] = "Borrow request submitted successfully. An admin will review it shortly.";
                        return RedirectToAction("MyBorrows");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
                }
            }

            vm.Books = bookRepo.GetAll();
            return View("RequestBook", vm);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Approve(int id)
        {
            BorrowRecord? record = borrowRepo.GetByIdWithDetails(id);
            if (record == null) return NotFound();

            if (record.Status != BorrowStatus.Pending)
            {
                TempData["Error"] = "This request has already been processed.";
                return RedirectToAction("PendingRequests");
            }

            BorrowApproveViewModel vm = new BorrowApproveViewModel
            {
                Id = record.Id,
                PickupDate = DateTime.Now
            };
            ViewBag.BookTitle = record.Book?.Title;
            ViewBag.MemberName = record.Member?.UserName;
            ViewBag.RequestedDate = record.RequestedDate;
            return View("Approve", vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Approve(BorrowApproveViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    BorrowRecord? record = borrowRepo.GetById(vm.Id);
                    if (record == null) return NotFound();

                    if (record.Status != BorrowStatus.Pending)
                    {
                        TempData["Error"] = "This request has already been processed.";
                        return RedirectToAction("PendingRequests");
                    }

                    record.BorrowDate = vm.PickupDate;
                    record.Status = BorrowStatus.Borrowed;
                    borrowRepo.Update(record);
                    borrowRepo.Save();
                    TempData["Success"] = "Borrow request approved successfully.";
                    return RedirectToAction("PendingRequests");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
                }
            }

            BorrowRecord? r = borrowRepo.GetByIdWithDetails(vm.Id);
            ViewBag.BookTitle = r?.Book?.Title;
            ViewBag.MemberName = r?.Member?.UserName;
            ViewBag.RequestedDate = r?.RequestedDate;
            return View("Approve", vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(int id)
        {
            BorrowRecord? record = borrowRepo.GetById(id);
            if (record == null) return NotFound();

            if (record.Status != BorrowStatus.Pending)
            {
                TempData["Error"] = "This request has already been processed.";
                return RedirectToAction("PendingRequests");
            }

            record.Status = BorrowStatus.Rejected;
            borrowRepo.Update(record);
            borrowRepo.Save();
            TempData["Success"] = "Borrow request rejected.";
            return RedirectToAction("PendingRequests");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            BorrowAddViewModel vm = new BorrowAddViewModel
            {
                Books = bookRepo.GetAll(),
                Members = userManager.Users.ToList()
            };
            return View("Create", vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BorrowAddViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    BorrowRecord record = new BorrowRecord
                    {
                        BookId = vm.BookId,
                        MemberId = vm.MemberId,
                        RequestedDate = DateTime.Now,
                        BorrowDate = DateTime.Now,
                        Status = BorrowStatus.Borrowed
                    };
                    borrowRepo.Add(record);
                    borrowRepo.Save();
                    TempData["Success"] = "Book borrowed successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
                }
            }

            vm.Books = bookRepo.GetAll();
            vm.Members = userManager.Users.ToList();
            return View("Create", vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult ReturnBook(int id)
        {
            BorrowRecord? record = borrowRepo.GetById(id);
            if (record == null) return NotFound();

            var now = DateTime.Now;
            record.ReturnDate = now;

            if (record.BorrowDate.HasValue)
            {
                const int maxBorrowDays = 14;
                var dueDate = record.BorrowDate.Value.AddDays(maxBorrowDays);
                if (now > dueDate)
                {
                    var overdueDays = (now - dueDate).Days;
                    decimal fineAmount = overdueDays * 1m;

                    record.Status = BorrowStatus.Overdue;
                    borrowRepo.Update(record);
                    borrowRepo.Save();

                    Fine fine = new Fine
                    {
                        Amount = fineAmount,
                        IsPaid = false,
                        BorrowRecordId = record.Id,
                        MemberId = record.MemberId
                    };
                    fineRepo.Add(fine);
                    fineRepo.Save();

                    TempData["Error"] = $"Book returned late by {overdueDays} day(s). Fine of ${fineAmount:F2} created.";
                }
                else
                {
                    record.Status = BorrowStatus.Returned;
                    borrowRepo.Update(record);
                    borrowRepo.Save();
                    TempData["Success"] = "Book returned successfully.";
                }
            }
            else
            {
                record.Status = BorrowStatus.Returned;
                borrowRepo.Update(record);
                borrowRepo.Save();
                TempData["Success"] = "Book returned successfully.";
            }

            return RedirectToAction("Index");
        }
    }
}