using System.ComponentModel.DataAnnotations;

namespace LibraryManagementAPI.DTOS
{
    /// <summary>
    /// Data required to register a new user account.
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// The user’s full name.
        /// </summary>
        [Required]
        public string FullName { get; set; }

        /// <summary>
        /// The user’s email address (will be their login).
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// The user’s password.
        /// </summary>
        [Required, MinLength(6)]
        public string Password { get; set; }

        /// <summary>
        /// The role to assign (e.g. “Student”, “Librarian”, “Admin”).
        /// </summary>
        [Required]
        public string Role { get; set; }
    }
}
