using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebApiAuthors.DTOs
{
    /// <summary>
    /// Respuesta a la Authenticación Correcta del Usuario
    /// </summary>
    public class AuthResponseDTO
    {
        /// <summary>
        /// Token del Usuario
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Fecha de Expiración, Caducidad del Token
        /// </summary>
        [Required]
        [Display(Name = "Expiración")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
        public DateTime? Experation { get; set; }
    }
}
