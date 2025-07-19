using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.DTOS
{
    /// <summary>
    /// Information needed to borrow a book.
    /// </summary>
    public class BorrowDto
    {
        /// <summary>The ID of the book to borrow.</summary>
        [Required]
        public int BookId { get; set; }

        /// <summary>
        /// (Admins/Librarians only) The user ID on whose behalf to borrow.
        /// </summary>
        public string? UserId { get; set; }
    }
}
