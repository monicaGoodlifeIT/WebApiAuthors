namespace WebApiAuthors.DTOs
{
    /// <summary>
    /// DTO de Libros con sus Autores
    /// </summary>
    public class BookDTOwithAuthors : BookDTO
    {
        /// <summary>
        /// Propiedad de Navegación --> Autores  Relación Muchos a Muchos, Libros/Autores
        /// </summary>
        public List<AuthorDTO>? Authors { get; set; }
    }
}
