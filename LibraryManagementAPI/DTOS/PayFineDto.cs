using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.DTOS
{
    /// <summary>
    /// Information needed to pay a library fine.
    /// </summary>
    public class PayFineDto
    {
        /// <summary>The ID of the fine to pay.</summary>
        [Required]
        public int FineId { get; set; }
    }
}
