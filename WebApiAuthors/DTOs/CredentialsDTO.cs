using System.ComponentModel.DataAnnotations;

namespace WebApiAuthors.DTOs
{
    /// <summary>
    /// Credenciales que envía el Usuario para Autenticarse
    /// </summary>
    public class CredentialsDTO
    {
        /// <summary>
        /// Email del Usuario, como username
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Contraseña de Usuario
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
