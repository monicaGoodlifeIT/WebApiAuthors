using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WebApiAuthors.Entities;
using WebApiAuthors.Validations;

namespace WebApiAuthors.DTOs
{
    public class BookAddDTO
    {
        /// <summary>
        /// Título del nuevo Libro
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
        /// Propiedad de navegación que conecta con la entidad AuthorBook  - Relación Muchos a Muchos
        /// </summary>
        public List<Guid>? AuthorsIds { get; set; }
    }
}
