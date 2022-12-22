using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WebApiAuthors.Validations;

namespace WebApiAuthors.DTOs
{
    /// <summary>
    /// DTO para HTTP Pach de Libro
    /// </summary>
    public class BookPatchDTO
    {
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
        /// Fecha de Publicación del Libro
        /// </summary>
        /// 
        [Required]
        [Display(Name = "Publicado")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? PublicationDate { get; set; }
    }
}
