using LibraryManagementAPI.DTOS;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _service;

        public BorrowController(IBorrowService service)
            => _service = service;

        /// <summary>
        /// Creates a new borrow record (loan).
        /// </summary>
        /// <param name="dto">Borrow details: BookId and optional UserId (for Admin/Librarian).</param>
        /// <returns>201 Created with the new record, or 400 on failure.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Borrow([FromBody] BorrowDto dto)
        {
            var record = await _service.BorrowBookAsync(dto, User);
            if (record == null)
                return BadRequest("Book is not available.");

            return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
        }

        /// <summary>
        /// Retrieves any borrow record by ID (Admins/Librarians only).
        /// </summary>
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Librarian")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _service.GetByIdAsync(id);
            if (record == null)
                return NotFound();

            return Ok(record);
        }

        /// <summary>
        /// Returns a borrowed book.
        /// </summary>
        /// <param name="dto">Return details: BorrowId.</param>
        /// <returns>204 No Content on success, 400/403/404 on error.</returns>
        [HttpPost("return")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Return([FromBody] ReturnDto dto)
        {
            var result = await _service.ReturnBookAsync(dto, User);
            if (!result)
                return BadRequest("Could not return the book.");

            return NoContent();
        }

        /// <summary>
        /// Retrieves the current user’s borrow history.
        /// </summary>
        [HttpGet("my")]
        [Authorize(Roles = "Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyHistory()
        {
            var history = await _service.GetMyHistoryAsync(User);
            return Ok(history);
        }
    }
}
