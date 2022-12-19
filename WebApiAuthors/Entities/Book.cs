namespace WebApiAuthors.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int AuthorId { get; set; }

        // Propiedad de Navegación
        public Author? Author { get; set; }
    }
}
