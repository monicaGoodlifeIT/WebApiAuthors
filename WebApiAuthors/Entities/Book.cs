using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WebApiAuthors.Validations;

namespace WebApiAuthors.Entities
{
    public class Book
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Título del libro
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Título")]
        [StringLength(maximumLength: 150, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres")]
        [FirstCapitalLetter]
        public string? Title { get; set; }

        /// <summary>
        /// Propiedad de Navegación - Listado de comentarios asociados al libro
        /// </summary>
        public List<Comment>? Comments { get; set; }
    }
}
