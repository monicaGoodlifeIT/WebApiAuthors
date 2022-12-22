namespace WebApiAuthors.Entities
{
    /// <summary>
    /// Entidad de Relación Muchos a Muchos --> Relación Autores y Libros
    /// </summary>
    public class AuthorBook
    {
        /// <summary>
        /// Identificador del Libro - PK Combinada
        /// </summary>
        public Guid BookId { get; set; }

        /// <summary>
        /// Identificador del Author - PK Combinada
        /// </summary>
        public Guid AuthorId { get; set; }

        /// <summary>
        /// Orden display cuando el libro tiene más de un autor
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Propiedad de Navegación --> Libro del Autor
        /// </summary>
        public Book? Book { get; set; }
        
        /// <summary>
        /// Propiedad de Navegación --> Autor del Libro
        /// </summary>
        public Author? Author { get; set; }
    }
}
