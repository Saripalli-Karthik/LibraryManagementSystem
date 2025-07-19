using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task<Book> CreateBookAsync(BookDto dto);
        Task<bool> UpdateBookAsync(int id, BookDto dto);
        Task<BookDeleteResult> DeleteBookAsync(int id);
    }
}