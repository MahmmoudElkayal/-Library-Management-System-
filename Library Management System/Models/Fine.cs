using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    public class Fine
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }

        public int BorrowRecordId { get; set; }
        [ForeignKey("BorrowRecordId")]
        public BorrowRecord? BorrowRecord { get; set; }

        public string MemberId { get; set; }
        [ForeignKey("MemberId")]
        public LibraryMember? Member { get; set; }
    }
}