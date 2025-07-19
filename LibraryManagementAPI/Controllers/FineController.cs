using LibraryManagementAPI.DTOS;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FineController : ControllerBase
    {
        private readonly IFineService _fineService;

        public FineController(IFineService fineService)
        {
            _fineService = fineService;
        }

        /// <summary>
        /// Retrieves fines:
        /// - Students see only their fines.
        /// - Admin/Librarian see all fines.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFines()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var isAdminOrLibrarian = User.IsInRole("Admin") || User.IsInRole("Librarian");

            var result = await _fineService.GetFinesAsync(userId, isAdminOrLibrarian);
            return Ok(result);
        }

        /// <summary>
        /// Pays a specific fine.
        /// </summary>
        [HttpPost("pay")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PayFine([FromBody] PayFineDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var isAdminOrLibrarian = User.IsInRole("Admin") || User.IsInRole("Librarian");

            var result = await _fineService.PayFineAsync(dto.FineId, userId, isAdminOrLibrarian);

            return result switch
            {
                "NotFound" => NotFound("Fine not found."),
                "AlreadyPaid" => BadRequest("Fine is already paid."),
                "Forbidden" => Forbid(),
                _ => NoContent()
            };
        }
    }
}
