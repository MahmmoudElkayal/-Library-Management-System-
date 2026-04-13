using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.ViewModels
{
    public class BorrowApproveViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pickup date is required")]
        [Display(Name = "Pickup Date")]
        [DataType(DataType.DateTime)]
        public DateTime PickupDate { get; set; }
    }
}