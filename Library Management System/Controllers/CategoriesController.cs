using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository categoryRepo;

        public CategoriesController(ICategoryRepository categoryRepo)
        {
            this.categoryRepo = categoryRepo;
        }

        public IActionResult Index()
        {
            List<Category> categories = categoryRepo.GetAll();
            return View("Index", categories);
        }

        public IActionResult Details(int id)
        {
            Category? category = categoryRepo.GetById(id);
            if (category == null) return NotFound();
            return View("Details", category);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create", new CategoryAddViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryAddViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Category category = new Category
                    {
                        Name = vm.Name,
                        Description = vm.Description
                    };
                    categoryRepo.Add(category);
                    categoryRepo.Save();
                    TempData["Success"] = "Category added successfully";
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
            Category? category = categoryRepo.GetById(id);
            if (category == null) return NotFound();

            CategoryEditViewModel vm = new CategoryEditViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
            return View("Edit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Category? category = categoryRepo.GetById(vm.Id);
                    if (category == null) return NotFound();

                    category.Name = vm.Name;
                    category.Description = vm.Description;
                    categoryRepo.Update(category);
                    categoryRepo.Save();
                    TempData["Success"] = "Category updated successfully";
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
            Category? category = categoryRepo.GetById(id);
            if (category != null)
            {
                categoryRepo.Delete(id);
                categoryRepo.Save();
                TempData["Success"] = "Category deleted successfully";
            }
            else
            {
                TempData["Error"] = "Category not found";
            }
            return RedirectToAction("Index");
        }
    }
}