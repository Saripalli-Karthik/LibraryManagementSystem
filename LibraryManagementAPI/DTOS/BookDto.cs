using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.DTOs
{
    /// <summary>
    /// Data needed to create or update a book.
    /// </summary>
    public class BookDto
    {
        /// <summary>
        /// The book’s title.
        /// </summary>
        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The book’s author.
        /// </summary>
        [Required, StringLength(100)]
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// The ISBN in 978-XXXXXXXXXX format.
        /// </summary>
        public string? ISBN { get; set; }

        /// <summary>
        /// The name of the publisher.
        /// </summary>
        public string? Publisher { get; set; }

        /// <summary>
        /// Year the book was published.
        /// </summary>
        [Range(1000, 2100)]
        public int PublishedYear { get; set; }

        /// <summary>
        /// Total number of copies available in the library.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int TotalCopies { get; set; }
    }
}
