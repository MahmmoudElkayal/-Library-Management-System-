# Library Management System — Style Guide

This document captures the exact coding conventions, patterns, and naming rules used throughout this project. All derived from analysis of the developer's reference projects (Instructor project, WebAppAEC46, E-MarketProject).

---

## Project Structure

```
LibraryManagementSystem/
  Controllers/
  Interfaces/
  Models/
  Repositories/
  ViewModels/
  Views/
    Account/
    Authors/
    Books/
    Borrow/
    Categories/
    Fines/
    Home/
    Member/
    Shared/
  wwwroot/
    images/
      books/
      members/
    css/
    js/
    lib/
```

---

## Naming Conventions

| Element | Convention | Example |
|---------|-----------|---------|
| Controllers | `{Entity}Controller` | `BooksController`, `AuthorsController` |
| Actions (GET) | `Index`, `Details`, `Create`, `Edit` | `public IActionResult Index()` |
| Actions (POST) | Same name as GET | `Create(VM)`, `Edit(VM)`, `Delete(id)` |
| Models | Singular PascalCase | `Book`, `BorrowRecord`, `LibraryMember` |
| ViewModels | Descriptive + `ViewModel` | `BookAddViewModel`, `EditProfileViewModel` |
| Repositories | `{Entity}Repository` | `BookRepository` |
| Interfaces | `I{Entity}Repository` | `IBookRepository` |
| PK fields | `Id` | `public int Id { get; set; }` |
| FK fields | `{Entity}Id` | `public int AuthorId { get; set; }` |
| Nav properties | Nullable with `?` | `public Author? Author { get; set; }` |
| Private fields | camelCase (no underscore) | `private readonly IBookRepository bookRepo;` |
| Connection string | `"cs"` | `"cs": "Data Source=.;..."` |

---

## Repository Pattern

### Generic Interface (`IRepository<T>`)
```csharp
public interface IRepository<T> where T : class
{
    List<T> GetAll();
    T? GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
    void Save();
}
```
- **All synchronous** (no async)
- `GetById` returns `T?` (nullable)
- `Delete` takes `int id`, fetches internally, null-checks before remove
- `Save()` calls `SaveChanges()` separately (not auto-saved)

### Update Pattern
Fetch tracked entity, copy properties, then call `Update()` + `Save()`:
```csharp
Book? book = bookRepo.GetById(vm.Id);
if (book == null) return NotFound();
book.Title = vm.Title;
bookRepo.Update(book);
bookRepo.Save();
```

### Entity-Specific Interfaces
Add methods with `.Include()` for eager loading:
```csharp
public interface IBookRepository : IRepository<Book>
{
    List<Book> GetAllWithDetails();
    Book? GetByIdWithDetails(int id);
}
```

---

## DbContext

- Inherits `IdentityDbContext<LibraryMember>` (not plain `DbContext`)
- Single constructor: `public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }`
- `OnModelCreating` for:
  - `base.OnModelCreating(modelBuilder)` call first
  - `OnDelete(DeleteBehavior.NoAction)` for all FK relationships to prevent cascade conflicts
  - `HasData()` for seed data (Categories, Authors)
- Connection string name: `"cs"`

---

## Controllers

### DI Pattern
```csharp
private readonly IBookRepository bookRepo;
public BooksController(IBookRepository bookRepo) { this.bookRepo = bookRepo; }
```

### CRUD Create Pattern
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Create(BookAddViewModel vm)
{
    if (ModelState.IsValid)
    {
        try
        {
            // map VM to entity
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
    // reload dropdown data before returning
    vm.Authors = authorRepo.GetAll();
    vm.Categories = categoryRepo.GetAll();
    return View("Create", vm);
}
```

### Auth
- `[Authorize(Roles = "Admin")]` at controller level for admin-only controllers
- `[Authorize(Roles = "Member")]` for member-only actions
- `[Authorize]` at controller level for authenticated-only controllers

### View Naming
- Always explicit: `return View("Index", model)` (not just `View(model)`)

---

## ViewModels

- **Validation attributes on ViewModels only** (not on Models)
- `IFormFile?` for file uploads
- `List<Xxx>` for dropdown data
- `[Range(1, int.MaxValue, ErrorMessage = "Please select a...")]` for dropdowns
- Edit ViewModels include `int Id` and `string? CurrentImage`

---

## Views

### Bootstrap 5 CDN
- CSS: `https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css`
- JS: `https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js`

### _ViewImports.cshtml
```cshtml
@using LibraryManagementSystem
@using LibraryManagementSystem.Models
@using LibraryManagementSystem.ViewModels
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```

### Tag Helpers (preferred)
- `asp-for`, `asp-validation-for`, `asp-validation-summary`
- `asp-controller`, `asp-action`, `asp-route-id`
- `asp-items="@(new SelectList(...))"` for dropdowns

### CSS Classes
- Form groups: `mb-3`
- Inputs: `form-control`
- Selects: `form-control`
- Labels: `form-label`
- Validation: `text-danger`
- Buttons: `btn btn-success` (Create), `btn btn-primary` (Save/Edit/Detail), `btn btn-warning` (Edit link), `btn btn-danger` (Delete), `btn btn-secondary` (Back/Cancel)
- Tables: `table table-bordered table-hover`, `thead class="table-dark"`
- Card-based layout for Homepage

### Partial Views
- `_BookForm.cshtml`, `_AuthorForm.cshtml`, `_CategoryForm.cshtml`
- Shared between Create and Edit views via `<partial name="_XxxForm" model="Model" />`

### Validation Scripts
- Include at bottom of forms via `@section Scripts { @{await Html.RenderPartialAsync("_ValidationScriptsPartial");} }`

---

## File Upload Pattern

```csharp
if (vm.CoverImage != null && vm.CoverImage.Length > 0)
{
    var dir = Path.Combine(env.WebRootPath, "images", "books");
    Directory.CreateDirectory(dir);
    var ext = Path.GetExtension(vm.CoverImage.FileName);
    var fileName = $"{Guid.NewGuid():N}{ext}";
    using (var stream = System.IO.File.Create(Path.Combine(dir, fileName)))
    {
        vm.CoverImage.CopyTo(stream);
    }
    book.CoverImage = fileName;
}
```
- Store only the filename in DB (not the full path)
- `Guid.NewGuid():N` for unique filenames
- Delete old file before replacing on edit

---

## Identity Setup

- `LibraryMember : IdentityUser` with custom fields (`Address`, `ProfileImage`)
- Relaxed password: min 6 chars, requires digit + uppercase, no special char required
- Role seeding in `Program.cs` using `app.Services.CreateScope()`
- Admin seed: `admin` / `Admin@123`
- Login accepts username or email (check email format)

---

## TempData Messages

```csharp
// In controllers:
TempData["Success"] = "Record added successfully";
TempData["Error"] = "Something went wrong";

// In layout (renders on every page):
@if (TempData["Success"] != null) { <div class="alert alert-success">...</div> }
@if (TempData["Error"] != null) { <div class="alert alert-danger">...</div> }
```

---

## Key Rules

1. **No async in repositories** — sync `GetAll`, `GetById`, `Add`, `Update`, `Delete`, `Save`
2. **Controllers use async/await** only for Identity operations (`UserManager`, `SignInManager`)
3. **`List<T>` not `IEnumerable<T>`** for repository return types
4. **`GetById` returns nullable `T?`**
5. **`Delete` checks null before remove**
6. **`Update` fetches tracked entity then copies fields** (not `Update(VM)` directly)
7. **ViewModels have direct entity property names** (not renamed like `CrsName`)
8. **`try/catch` in POST actions** with `ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message)`
9. **`ViewData` or SelectList for dropdowns**
10. **Bootstrap 5 CDN** for styling
11. **Connection string name: `"cs"`**
12. **`Guid.NewGuid():N` for uploaded filenames**
13. **`[Authorize(Roles = "...")]`** for role-based access
14. **`TempData`** for success/error messages after redirects
15. **Partial views** for shared form sections between Create/Edit