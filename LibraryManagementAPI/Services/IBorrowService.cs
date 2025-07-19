using LibraryManagementAPI.DTOS;
using LibraryManagementAPI.Models;
using System.Security.Claims;

public interface IBorrowService
{
    Task<BorrowRecord?> BorrowBookAsync(BorrowDto dto, ClaimsPrincipal user);
    Task<BorrowRecord?> GetByIdAsync(int id);
    Task<bool> ReturnBookAsync(ReturnDto dto, ClaimsPrincipal user);
    Task<List<BorrowRecordDto>> GetMyHistoryAsync(ClaimsPrincipal user);
}