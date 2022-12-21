using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WebApiAuthors.Validations;

namespace WebApiAuthors.DTOs
{
    /// <summary>
    /// DTO POST para Entity BookCollection, sentido DB --> API (POST)
    /// </summary>
    public class BookCollectionAddDTO
    {
        /// <summary>
        /// Título de la Colección
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Colección")]
        [StringLength(maximumLength: 100, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres")]
        [FirstCapitalLetter]
        public string? Title { get; set; }

        /// <summary>
        /// Género literaría al que pertenece la Colección
        /// </summary>
        [Display(Name = "Género")]
        public GenreType? Genre { get; set; }

        /// <summary>
        /// Fecha de creación del registro de la Colección
        /// </summary>
        [Required]
        [Display(Name = "Registro")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
        public DateTime? RegisterCreated { get; set; } = default(DateTime?);
    }
}
