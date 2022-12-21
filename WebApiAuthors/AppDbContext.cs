using Microsoft.EntityFrameworkCore;
using WebApiAuthors.Entities;

namespace WebApiAuthors
{
    /// <summary>
    /// DBContext de la WebAPI
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public AppDbContext(DbContextOptions options) : base(options) {}

        /// <summary>
        /// Conexión con tabla de Autores
        /// </summary>
        public DbSet<Author>? Authors { get; set; } = null!;

        /// <summary>
        /// Conexión con tabla de Colecciones
        /// </summary>
        public DbSet<BookCollection>? BookCollections { get; set; } = null!;

        /// <summary>
        /// Conexión con tabla de Libros
        /// </summary>
        public DbSet<Book>? Books { get; set; } = null!;

        /// <summary>
        /// Conexión con tabla de Comentarios
        /// </summary>
        public DbSet<Comment>? Comments { get; set; } = null!;
    }
}
