using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required]
        [MaxLength(100)]
        public string Author { get; set; }

        public string? ISBN { get; set; }

        public string? Publisher { get; set; }

        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int PublishedYear { get; set; }

        public ICollection<BorrowRecord>? BorrowRecords { get; set; }
    }

}
