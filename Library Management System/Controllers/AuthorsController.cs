using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository authorRepo;

        public AuthorsController(IAuthorRepository authorRepo)
        {
            this.authorRepo = authorRepo;
        }

        public IActionResult Index()
        {
            List<Author> authors = authorRepo.GetAll();
            return View("Index", authors);
        }

        public IActionResult Details(int id)
        {
            Author? author = authorRepo.GetById(id);
            if (author == null) return NotFound();
            return View("Details", author);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create", new AuthorAddViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AuthorAddViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Author author = new Author
                    {
                        Name = vm.Name,
                        Bio = vm.Bio
                    };
                    authorRepo.Add(author);
                    authorRepo.Save();
                    TempData["Success"] = "Author added successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
                }
            }
            return View("Create", vm);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Author? author = authorRepo.GetById(id);
            if (author == null) return NotFound();

            AuthorEditViewModel vm = new AuthorEditViewModel
            {
                Id = author.Id,
                Name = author.Name,
                Bio = author.Bio
            };
            return View("Edit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AuthorEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Author? author = authorRepo.GetById(vm.Id);
                    if (author == null) return NotFound();

                    author.Name = vm.Name;
                    author.Bio = vm.Bio;
                    authorRepo.Update(author);
                    authorRepo.Save();
                    TempData["Success"] = "Author updated successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
                }
            }
            return View("Edit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Author? author = authorRepo.GetById(id);
            if (author != null)
            {
                authorRepo.Delete(id);
                authorRepo.Save();
                TempData["Success"] = "Author deleted successfully";
            }
            else
            {
                TempData["Error"] = "Author not found";
            }
            return RedirectToAction("Index");
        }
    }
}