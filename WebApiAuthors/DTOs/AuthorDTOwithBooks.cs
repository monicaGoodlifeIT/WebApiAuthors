namespace WebApiAuthors.DTOs
{
    /// <summary>
    /// DTO de Authores con sus Libros
    /// </summary>
    public class AuthorDTOwithBooks : AuthorDTO
    {
        /// <summary>
        /// Propiedad de Navegación --> Libros  Relación Muchos a Muchos, Libros/Autores
        /// </summary>
        public List<BookDTO>? Books { get; set; }
    }
}
