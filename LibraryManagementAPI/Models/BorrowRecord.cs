using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.Models
{
    public class BorrowRecord
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }  // Navigation property

        [Required]
        public int BookId { get; set; }

        public Book Book { get; set; }

        public DateTime BorrowedAt { get; set; } = DateTime.UtcNow;

        public DateTime DueDate { get; set; }

        public DateTime? ReturnedAt { get; set; }

        public Fine? Fine { get; set; }
    }

}
