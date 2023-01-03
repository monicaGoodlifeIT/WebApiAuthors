using System.ComponentModel.DataAnnotations;

namespace WebApiAuthors.DTOs
{
    /// <summary>
    /// Asignación de Permisos de Asministrador
    /// </summary>
    public class AdminEditDTO
    {
        /// <summary>
        /// Email del uaurio al que se asignará, permisos de Administrador
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
    }
}
