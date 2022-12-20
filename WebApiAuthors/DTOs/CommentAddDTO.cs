using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WebApiAuthors.Entities;

namespace WebApiAuthors.DTOs
{
    /// <summary>
    /// DTO para añadir Comentario
    /// </summary>
    public class CommentAddDTO
    {
        /// <summary>
        /// Contenido del Comentario
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Comentario")]
        [StringLength(maximumLength: 255, ErrorMessage = "El campo {0} no debe tener más de {1} caracteres")]
        public string? Content { get; set; }

        //public Guid BookID { get; set; } // No es necesario porque ya está en la ruta del controlador

        /// <summary>
        /// Fecha de creación del comentario
        /// </summary>
        /// 
        [Required]
        [Display(Name = "Creado")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
        public DateTime? CreatedDate { get; set; } = default(DateTime?);
    }
}
