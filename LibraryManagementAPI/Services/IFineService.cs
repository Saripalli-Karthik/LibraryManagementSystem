using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Services
{
    public interface IFineService
    {
        Task<IEnumerable<object>> GetFinesAsync(string userId, bool isAdminOrLibrarian);
        Task<string> PayFineAsync(int fineId, string userId, bool isAdminOrLibrarian);
    }
}
