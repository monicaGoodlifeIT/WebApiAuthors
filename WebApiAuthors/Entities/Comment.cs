using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WebApiAuthors.Validations;

namespace WebApiAuthors.Entities
{
    /// <summary>
    /// Entidad de Comentario
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Contenido del Comentario
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Comentario")]
        [StringLength(maximumLength: 255, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres")]
        public string Content { get; set; } = null!;

        /// <summary>
        /// Identificador del libro al que pertenece el coemntario
        /// </summary>
        public Guid BookID  { get; set; }

        /// <summary>
        /// Fecha de creación del comentario
        /// </summary>
        /// 
        [Required]
        [Display(Name = "Creado")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
        public DateTime? CreatedDate { get; set; } = default(DateTime?);

        /// <summary>
        /// Propiedad de Navegación - Data del Libro asociado al comentario
        /// </summary>
        public Book Libro { get; set; } = null!;

        /// <summary>
        /// Identificador del usuario que ha creado el comentario
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Propiedad de navegación  - Data del Usuario asociado al comentario
        /// </summary>
        public IdentityUser User { get; set; } = null!;
    }
}
