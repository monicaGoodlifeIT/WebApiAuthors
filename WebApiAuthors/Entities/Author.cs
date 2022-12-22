using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAuthors.Validations;

namespace WebApiAuthors.Entities
{
    /// <summary>
    /// Entidad de Autor
    /// </summary>
    public class Author 
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre Completo del Autor
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres")]
        [FirstCapitalLetter]
        public string? Name { get; set; }

        /// <summary>
        /// Propiedad de Navegación - Relación Tabla AuthorBook, Muchos a Muchos
        /// </summary>
        public List<AuthorBook>? AuthorsBooks { get; set; }
    }
}