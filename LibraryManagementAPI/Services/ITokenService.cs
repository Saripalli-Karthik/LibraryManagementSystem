using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Services
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user, IList<string> roles);
    }
}
