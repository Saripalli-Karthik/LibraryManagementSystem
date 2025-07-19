using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.DTOS
{
    /// <summary>
    /// Information needed to return a borrowed book.
    /// </summary>
    public class ReturnDto
    {
        /// <summary>The ID of the borrow record being returned.</summary>
        [Required]
        public int BorrowId { get; set; }
    }
}
