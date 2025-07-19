using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Services
{
    public enum BookDeleteResult
    {
        Success,
        NotFound,
        HasBorrows
    }

    public class BookService : IBookService
    {
        private readonly AppDbContext _context;

        public BookService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<Book> CreateBookAsync(BookDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                ISBN = dto.ISBN,
                Publisher = dto.Publisher,
                PublishedYear = dto.PublishedYear,
                TotalCopies = dto.TotalCopies,
                AvailableCopies = dto.TotalCopies,
                CreatedAt = DateTime.UtcNow
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> UpdateBookAsync(int id, BookDto dto)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            var diff = dto.TotalCopies - book.TotalCopies;

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.ISBN = dto.ISBN;
            book.Publisher = dto.Publisher;
            book.PublishedYear = dto.PublishedYear;
            book.TotalCopies = dto.TotalCopies;
            book.AvailableCopies = Math.Max(0, book.AvailableCopies + diff);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<BookDeleteResult> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return BookDeleteResult.NotFound;

            bool hasBorrows = await _context.BorrowRecords.AnyAsync(br => br.BookId == id);
            if (hasBorrows)
                return BookDeleteResult.HasBorrows;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return BookDeleteResult.Success;
        }
    }
}
