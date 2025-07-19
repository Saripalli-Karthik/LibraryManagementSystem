using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
            => _bookService = bookService;

        /// <summary>
        /// Retrieves all books in the catalog.
        /// </summary>
        /// <returns>A list of all books.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        /// <summary>
        /// Retrieves a specific book by its ID.
        /// </summary>
        /// <param name="id">The ID of the book to retrieve.</param>
        /// <returns>The book if found; otherwise, 404.</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        /// <summary>
        /// Creates a new book.
        /// </summary>
        /// <param name="dto">The book data to create.</param>
        /// <returns>201 Created if successful; 400 Bad Request if invalid.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Librarian")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] BookDto dto)
        {
            var book = await _bookService.CreateBookAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }

        /// <summary>
        /// Updates an existing book.
        /// </summary>
        /// <param name="id">The ID of the book to update.</param>
        /// <param name="dto">The updated book data.</param>
        /// <returns>204 No Content if successful; 404 Not Found if the book does not exist.</returns>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] BookDto dto)
        {
            var success = await _bookService.UpdateBookAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Deletes a book by ID.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <returns>204 No Content if deleted; 400 Bad Request if borrow records exist; 404 if not found.</returns>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bookService.DeleteBookAsync(id);
            return result switch
            {
                BookDeleteResult.NotFound => NotFound(),
                BookDeleteResult.HasBorrows => BadRequest(new { message = "Cannot delete book: it has borrow records." }),
                _ => NoContent()
            };
        }
    }
}
