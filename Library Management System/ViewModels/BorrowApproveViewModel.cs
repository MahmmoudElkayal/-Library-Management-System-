using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.ViewModels
{
    public class BorrowApproveViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pickup date is required")]
        [Display(Name = "Pickup Date")]
        public DateTime PickupDate { get; set; }

        public string? BookTitle { get; set; }
        public string? MemberName { get; set; }
        public DateTime RequestedDate { get; set; }
    }
}