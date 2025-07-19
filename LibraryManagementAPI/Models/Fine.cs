using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.Models
{
    public class Fine
    {
        public int Id { get; set; }

        [Required]
        public int BorrowRecordId { get; set; }

        public BorrowRecord BorrowRecord { get; set; }

        public decimal Amount { get; set; }

        public bool IsPaid { get; set; } = false;

        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

        public DateTime? PaidAt { get; set; }
    }

}
