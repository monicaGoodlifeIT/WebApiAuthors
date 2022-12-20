using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WebApiAuthors.Entities;
using WebApiAuthors.Validations;

namespace WebApiAuthors.DTOs
{
    public class BookDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Título")]
        [StringLength(maximumLength: 150, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres")]
        [FirstCapitalLetter]
        public string? Title { get; set; }

        // Propiedad de Navegación
        public List<CommentDTO>? Comments { get; set; }
    }
}
