# Library Management System

A full-featured web application for managing a library's books, authors, categories, borrowing workflow, and member accounts. Built with **ASP.NET Core MVC** on **.NET 10.0**.

## Features

### Authentication & Authorization
- Role-based access control with **Admin** and **Member** roles
- Login by username or email
- User registration with profile image upload
- Secure password policy (min 8 chars, requires uppercase, lowercase, digit, and special character)
- Session management with 30-minute idle timeout

### Admin Dashboard
- **Books** — Full CRUD with cover image upload (type-validated, max 2MB)
- **Authors** — Full CRUD with bio management
- **Categories** — Full CRUD with descriptions
- **Borrow Records** — View all borrows, approve/reject member requests, direct borrow assignment, process returns
- **Fines** — Create manual fines, mark fines as paid, auto-generated fines for overdue returns ($1/day after 14 days)

### Member Portal
- Browse the book catalog
- Request books to borrow
- View personal borrow history and status
- Edit profile (username, email, address, profile image)

### Borrow Workflow
1. Member requests a book → status: `Pending`
2. Admin reviews and approves → status: `Borrowed` (or rejects)
3. Member returns the book → status: `Returned`
4. If returned late (>14 days), a fine is auto-generated → status: `Overdue`

## Tech Stack

| Layer | Technology |
|-------|-----------|
| **Framework** | ASP.NET Core MVC (.NET 10.0) |
| **Language** | C# with nullable reference types |
| **ORM** | Entity Framework Core 10.0.5 |
| **Database** | Microsoft SQL Server |
| **Authentication** | ASP.NET Core Identity |
| **Frontend** | Bootstrap 5.3.0, jQuery, jQuery Validation |
| **Architecture** | Repository Pattern with Dependency Injection |

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server) (Express or Developer edition)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) with C# Dev Kit

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/MahmmoudElkayal/-Library-Management-System-.git
cd "Library Management System"
```

### 2. Configure the Database

Edit `Library Management System/appsettings.json` and update the connection string to match your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "cs": "Data Source=.;Initial Catalog=LibraryMgmt;Integrated Security=True;Encrypt=False;Trust Server Certificate=True"
  }
}
```

| Parameter | Description |
|-----------|-------------|
| `Data Source` | Your SQL Server instance name (use `.` for local default, `(localdb)\MSSQLLocalDB` for LocalDB) |
| `Initial Catalog` | Database name |
| `Integrated Security` | Use Windows authentication |

### 3. Set the Admin Password

The admin account password is configured via the `AdminPassword` setting. For development, set it in `appsettings.Development.json`:

```json
{
  "AdminPassword": "YourStrongPassword123!"
}
```

For production, use an environment variable:

```bash
setx AdminPassword "YourStrongPassword123!"
```

### 4. Apply Database Migrations

```bash
cd "Library Management System"
dotnet ef database update
```

This creates the database, tables, and seeds initial data (4 categories and 4 authors).

### 5. Run the Application

**Visual Studio:**
1. Open `Library Management System.slnx`
2. Select the `http` launch profile
3. Press **F5** or click **Start**

**Command Line:**
```bash
cd "Library Management System"
dotnet run
```

The application will be available at `http://localhost:5226`.

## Default Admin Account

| Field | Value |
|-------|-------|
| **Username** | `admin` |
| **Email** | `admin@library.local` |
| **Password** | Value of `AdminPassword` setting |
| **Role** | Admin |

The admin account is created automatically on first run if it doesn't exist.

## Project Structure

```
Library Management System/
├── Attributes/              # Custom validation attributes
│   └── AllowedFileExtensionsAttribute.cs
├── Controllers/             # MVC controllers
│   ├── AccountController.cs    # Login, Register, Logout
│   ├── AuthorsController.cs    # Author CRUD (Admin)
│   ├── BooksController.cs      # Book CRUD with file upload (Admin)
│   ├── BorrowController.cs     # Borrow workflow (Admin + Member)
│   ├── CategoriesController.cs # Category CRUD (Admin)
│   ├── FinesController.cs      # Fine management (Admin)
│   ├── HomeController.cs       # Home page + Admin Dashboard
│   └── MemberController.cs     # Member profile editing
├── Interfaces/              # Repository interfaces
│   ├── IRepository.cs
│   ├── IBookRepository.cs
│   ├── IAuthorRepository.cs
│   ├── ICategoryRepository.cs
│   ├── IBorrowRepository.cs
│   └── IFineRepository.cs
├── Migrations/              # EF Core migrations
├── Models/                  # Entity models
│   ├── LibraryDbContext.cs     # DbContext + seed data
│   ├── Book.cs
│   ├── Author.cs
│   ├── Category.cs
│   ├── BorrowRecord.cs
│   ├── BorrowStatus.cs         # Enum for borrow states
│   ├── Fine.cs
│   ├── LibraryMember.cs        # IdentityUser extension
│   └── ErrorViewModel.cs
├── Properties/
│   └── launchSettings.json
├── Repositories/            # Repository implementations
│   ├── BookRepository.cs
│   ├── AuthorRepository.cs
│   ├── CategoryRepository.cs
│   ├── BorrowRepository.cs
│   └── FineRepository.cs
├── ViewModels/              # View-specific models
├── Views/                   # Razor views
│   ├── Shared/
│   │   ├── _AdminLayout.cshtml
│   │   ├── _MemberLayout.cshtml
│   │   └── _LoginPartial.cshtml
│   └── ...
├── wwwroot/                 # Static files (CSS, JS, images)
├── appsettings.json
├── appsettings.Development.json
├── Program.cs               # Application entry point
└── LibraryManagementSystem.csproj
```

## Database Schema

### Core Tables

| Table | Description |
|-------|-------------|
| **Books** | Title, ISBN, CoverImage, AuthorId (FK), CategoryId (FK) |
| **Authors** | Name, Bio |
| **Categories** | Name, Description |
| **BorrowRecords** | BorrowDate, ReturnDate, RequestedDate, Status (enum), MemberId (FK), BookId (FK) |
| **Fines** | Amount (decimal 18,2), IsPaid, BorrowRecordId (FK), MemberId (FK) |
| **AspNetUsers** | Identity users extended with Address and ProfileImage (LibraryMember) |
| **AspNetRoles** | Identity roles (Admin, Member) |

### Borrow Status Enum

```csharp
public enum BorrowStatus
{
    Pending,    // Request submitted, awaiting admin approval
    Borrowed,   // Approved and checked out
    Returned,   // Successfully returned on time
    Overdue,    // Returned late, fine generated
    Rejected    // Request denied by admin
}
```

## Security

- **Password Policy**: Minimum 8 characters, requires uppercase, lowercase, digit, and special character
- **File Upload Validation**: Only `.jpg`, `.jpeg`, `.png`, `.gif` files under 2MB are accepted
- **Anti-Forgery Tokens**: All POST actions protected against CSRF
- **Role-Based Authorization**: Admin-only actions protected with `[Authorize(Roles = "Admin")]`
- **Admin Password**: Never hardcoded; configured via environment variable or app setting
- **Nullable Reference Types**: Enabled project-wide to prevent null reference exceptions

## Development

### Adding a New Migration

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Running Tests

*(No test project currently exists. Consider adding xUnit tests for repositories and controllers.)*

## License

This project is for educational purposes.

## Acknowledgments

- Built with [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet)
- UI framework: [Bootstrap 5](https://getbootstrap.com/)
- ORM: [Entity Framework Core](https://docs.microsoft.com/ef/core/)
