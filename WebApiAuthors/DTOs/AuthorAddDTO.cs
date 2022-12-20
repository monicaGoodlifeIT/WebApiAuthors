using System.ComponentModel.DataAnnotations;
using WebApiAuthors.Validations;

namespace WebApiAuthors.DTOs
{
    public class AuthorAddDTO
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres")]
        [FirstCapitalLetter]
        public string? Name { get; set; }
    }
}
