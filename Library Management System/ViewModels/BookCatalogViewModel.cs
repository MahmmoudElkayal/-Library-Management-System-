using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManagementSystem.ViewModels
{
    public class BookCatalogViewModel
    {
        public List<Book> Books { get; set; } = new List<Book>();
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Authors { get; set; } = new List<SelectListItem>();
        
        public string? SearchString { get; set; }
        public int? SelectedCategoryId { get; set; }
        public int? SelectedAuthorId { get; set; }
        public string? SortOrder { get; set; }
        public string? ViewMode { get; set; } = "grid"; // "grid" or "list"
        
        public int TotalBooks { get; set; }
        public int AvailableBooks { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public int TotalPages => (int)Math.Ceiling(TotalBooks / (double)PageSize);
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
