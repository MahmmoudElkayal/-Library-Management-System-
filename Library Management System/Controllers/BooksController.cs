using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManagementSystem.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository bookRepo;
        private readonly IAuthorRepository authorRepo;
        private readonly ICategoryRepository categoryRepo;
        private readonly IWebHostEnvironment env;

        public BooksController(IBookRepository bookRepo, IAuthorRepository authorRepo, ICategoryRepository categoryRepo, IWebHostEnvironment env)
        {
            this.bookRepo = bookRepo;
            this.authorRepo = authorRepo;
            this.categoryRepo = categoryRepo;
            this.env = env;
        }

        public IActionResult Index(string? searchString, int? categoryId, int? authorId, string? sortOrder, string? viewMode, int page = 1)
        {
            var books = bookRepo.SearchBooks(searchString, categoryId, authorId);

            // Apply sorting
            books = sortOrder switch
            {
                "title_desc" => books.OrderByDescending(b => b.Title).ToList(),
                "author" => books.OrderBy(b => b.Author?.Name).ToList(),
                "author_desc" => books.OrderByDescending(b => b.Author?.Name).ToList(),
                "category" => books.OrderBy(b => b.Category?.Name).ToList(),
                _ => books.OrderBy(b => b.Title).ToList()
            };

            var totalBooks = books.Count;
            var availableBooks = bookRepo.GetAvailableBooks().Count;

            // Apply pagination
            books = books.Skip((page - 1) * 12).Take(12).ToList();

            var viewModel = new BookCatalogViewModel
            {
                Books = books,
                Categories = categoryRepo.GetAll().Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList(),
                Authors = authorRepo.GetAll().Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList(),
                SearchString = searchString,
                SelectedCategoryId = categoryId,
                SelectedAuthorId = authorId,
                SortOrder = sortOrder,
                ViewMode = viewMode ?? "grid",
                TotalBooks = totalBooks,
                AvailableBooks = availableBooks,
                CurrentPage = page,
                PageSize = 12
            };

            return View("Index", viewModel);
        }

        public IActionResult Details(int id)
        {
            Book? book = bookRepo.GetByIdWithDetails(id);
            if (book == null) return NotFound();
            return View("Details", book);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            BookAddViewModel vm = new BookAddViewModel
            {
                Authors = authorRepo.GetAll(),
                Categories = categoryRepo.GetAll()
            };
            return View("Create", vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookAddViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Book book = new Book
                    {
                        Title = vm.Title,
                        ISBN = vm.ISBN,
                        AuthorId = vm.AuthorId,
                        CategoryId = vm.CategoryId
                    };

                    if (vm.CoverImage != null && vm.CoverImage.Length > 0)
                    {
                        var booksDir = Path.Combine(env.WebRootPath, "images", "books");
                        Directory.CreateDirectory(booksDir);
                        var ext = Path.GetExtension(vm.CoverImage.FileName);
                        var fileName = $"{Guid.NewGuid():N}{ext}";
                        using (var stream = System.IO.File.Create(Path.Combine(booksDir, fileName)))
                        {
                            vm.CoverImage.CopyTo(stream);
                        }
                        book.CoverImage = fileName;
                    }

                    bookRepo.Add(book);
                    bookRepo.Save();
                    TempData["Success"] = "Book added successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
                }
            }

            vm.Authors = authorRepo.GetAll();
            vm.Categories = categoryRepo.GetAll();
            return View("Create", vm);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            Book? book = bookRepo.GetById(id);
            if (book == null) return NotFound();

            BookEditViewModel vm = new BookEditViewModel
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                CurrentCoverImage = book.CoverImage,
                Authors = authorRepo.GetAll(),
                Categories = categoryRepo.GetAll()
            };
            return View("Edit", vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Book? book = bookRepo.GetById(vm.Id);
                    if (book == null) return NotFound();

                    book.Title = vm.Title;
                    book.ISBN = vm.ISBN;
                    book.AuthorId = vm.AuthorId;
                    book.CategoryId = vm.CategoryId;

                    if (vm.CoverImage != null && vm.CoverImage.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(book.CoverImage))
                        {
                            var oldPath = Path.Combine(env.WebRootPath, "images", "books", book.CoverImage);
                            if (System.IO.File.Exists(oldPath))
                            {
                                System.IO.File.Delete(oldPath);
                            }
                        }

                        var booksDir = Path.Combine(env.WebRootPath, "images", "books");
                        Directory.CreateDirectory(booksDir);
                        var ext = Path.GetExtension(vm.CoverImage.FileName);
                        var fileName = $"{Guid.NewGuid():N}{ext}";
                        using (var stream = System.IO.File.Create(Path.Combine(booksDir, fileName)))
                        {
                            vm.CoverImage.CopyTo(stream);
                        }
                        book.CoverImage = fileName;
                    }

                    bookRepo.Update(book);
                    bookRepo.Save();
                    TempData["Success"] = "Book updated successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
                }
            }

            vm.Authors = authorRepo.GetAll();
            vm.Categories = categoryRepo.GetAll();
            return View("Edit", vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Book? book = bookRepo.GetById(id);
            if (book != null)
            {
                if (!string.IsNullOrEmpty(book.CoverImage))
                {
                    var imagePath = Path.Combine(env.WebRootPath, "images", "books", book.CoverImage);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                bookRepo.Delete(id);
                bookRepo.Save();
                TempData["Success"] = "Book deleted successfully";
            }
            return RedirectToAction("Index");
        }
    }
}