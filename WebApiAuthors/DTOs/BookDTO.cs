using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WebApiAuthors.Entities;
using WebApiAuthors.Validations;

namespace WebApiAuthors.DTOs
{
    /// <summary>
    /// DTO Libros
    /// </summary>
    public class BookDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Título del Libro
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Título")]
        [StringLength(maximumLength: 150, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres")]
        [FirstCapitalLetter]
        public string? Title { get; set; }

        /// <summary>
        /// Identificador de la Colección al que pertenece el libro
        /// </summary>
        public Guid BookCollectionID { get; set; }

        /// <summary>
        /// Orden de lectura del libro en la Collección
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Orden")]
        [Range(0, 50, ErrorMessage = "El campo {0} acepta valores entre {1} y {2}")]
        public int Order { get; set; }

        /// <summary>
        /// Propiedad de Navegación --> Comentarios Relación Uno a Muchos, Libro/Comentarios
        /// </summary>
        public List<CommentDTO>? Comments { get; set; }
    } 
}
