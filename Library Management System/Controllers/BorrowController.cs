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
        private readonly INotificationRepository notificationRepo;
        private readonly UserManager<LibraryMember> userManager;

        public BorrowController(IBorrowRepository borrowRepo, IBookRepository bookRepo, IFineRepository fineRepo, INotificationRepository notificationRepo, UserManager<LibraryMember> userManager)
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
        public IActionResult RequestBook(int? bookId)
        {
            BorrowRequestViewModel vm = new BorrowRequestViewModel
            {
                Books = bookRepo.GetAll(),
                BookId = bookId ?? 0
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
                    BorrowRecord record = new BorrowRecord
                    {
                        BookId = vm.BookId,
                        MemberId = memberId,
                        RequestedDate = DateTime.Now,
                        BorrowDate = null,
                        ReturnDate = null,
                        Status = "Pending"
                    };
                    borrowRepo.Add(record);
                    borrowRepo.Save();
                    TempData["Success"] = "Borrow request submitted successfully. An admin will review it shortly.";
                    return RedirectToAction("MyBorrows");
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

            if (record.Status != "Pending")
            {
                TempData["Error"] = "This request has already been processed.";
                return RedirectToAction("PendingRequests");
            }

            BorrowApproveViewModel vm = new BorrowApproveViewModel
            {
                Id = record.Id,
                PickupDate = DateTime.Now,
                BookTitle = record.Book?.Title,
                MemberName = record.Member?.UserName,
                RequestedDate = record.RequestedDate
            };
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

                    if (record.Status != "Pending")
                    {
                        TempData["Error"] = "This request has already been processed.";
                        return RedirectToAction("PendingRequests");
                    }

                    record.BorrowDate = vm.PickupDate;
                    record.Status = "Borrowed";
                    borrowRepo.Update(record);
                    borrowRepo.Save();

                    var bookForNotif = bookRepo.GetById(record.BookId);
                    notificationRepo.Add(new Notification
                    {
                        UserId = record.MemberId,
                        Message = $"Your borrow request for \"{bookForNotif?.Title}\" has been approved! Pickup date: {vm.PickupDate:MMM dd, yyyy}.",
                        Type = "Approval",
                        IsRead = false,
                        CreatedAt = DateTime.Now,
                        BorrowRecordId = record.Id
                    });
                    notificationRepo.Save();

                    TempData["Success"] = "Borrow request approved successfully.";
                    return RedirectToAction("PendingRequests");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
                }
            }

            BorrowRecord? r = borrowRepo.GetByIdWithDetails(vm.Id);
            if (r != null)
            {
                vm.BookTitle = r.Book?.Title;
                vm.MemberName = r.Member?.UserName;
                vm.RequestedDate = r.RequestedDate;
            }
            return View("Approve", vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(int id)
        {
            BorrowRecord? record = borrowRepo.GetById(id);
            if (record == null) return NotFound();

            if (record.Status != "Pending")
            {
                TempData["Error"] = "This request has already been processed.";
                return RedirectToAction("PendingRequests");
            }

            record.Status = "Rejected";
            borrowRepo.Update(record);
            borrowRepo.Save();

            var bookForNotif = bookRepo.GetById(record.BookId);
            notificationRepo.Add(new Notification
            {
                UserId = record.MemberId,
                Message = $"Your borrow request for \"{bookForNotif?.Title}\" has been rejected.",
                Type = "Rejection",
                IsRead = false,
                CreatedAt = DateTime.Now,
                BorrowRecordId = record.Id
            });
            notificationRepo.Save();

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
                        Status = "Borrowed"
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

                    record.Status = "Overdue";
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
                    record.Status = "Returned";
                    borrowRepo.Update(record);
                    borrowRepo.Save();
                    TempData["Success"] = "Book returned successfully.";
                }
            }
            else
            {
                record.Status = "Returned";
                borrowRepo.Update(record);
                borrowRepo.Save();
                TempData["Success"] = "Book returned successfully.";
            }

            return RedirectToAction("Index");
        }
    }
}