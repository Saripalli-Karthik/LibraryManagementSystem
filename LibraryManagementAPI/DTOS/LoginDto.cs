using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.DTOS
{
    /// <summary>
    /// Credentials used to obtain a JWT token.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// The user’s email address.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// The user’s password.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
