using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using WebApiAuthors.Validations;

namespace WebApiAuthors.Entities
{
    /// <summary>
    /// Géneros Literarios
    /// </summary>
    public enum GenreType
    {
        /// <summary>
        /// Novela Gráfica
        /// </summary>
        [Display(Name = "Novela Gráfica")]
        GraphicNovel,
        /// <summary>
        /// Acción y Misterio
        /// </summary>
        [Display(Name = "Acción")]
        Action,
        /// <summary>
        /// Crimén y Policíacas
        /// </summary>
        [Display(Name = "Crimen")]
        Crime,
        /// <summary>
        /// Relatos Cortos
        /// </summary>
        [Display(Name = "Relato Corto")]
        ShortStory,
        /// <summary>
        /// Ciencia Ficción
        /// </summary>
        [Display(Name = "Ciencia Ficción")]
        ScienceFiction,
        /// <summary>
        /// Comedia
        /// </summary>
        [Display(Name = "Comedia")]
        Comedy,   
        /// <summary>
        /// Fantasía
        /// </summary>
        [Display(Name = "Fantasia")]
        Fantasy,
        /// <summary>
        /// Histórico
        /// </summary>
        [Display(Name = "Histórico")]
        History
    }

    /// <summary>
    /// Entidad se Colecciones
    /// </summary>
    public class BookCollection
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public Guid Id { get; set; }

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
        /// 
        [Required]
        [Display(Name = "Registro")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
        public DateTime? RegisterCreated { get; set; } = default(DateTime?);

        /// <summary>
        /// Propiedad de Navegación - Listado de libros asociados a la Colección
        /// </summary>
        public List<Book>? Books { get; set; }
    }
}
