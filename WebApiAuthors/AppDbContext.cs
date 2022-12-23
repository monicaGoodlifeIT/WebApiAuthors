using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiAuthors.Entities;

namespace WebApiAuthors
{
    /// <summary>
    /// DBContext de la WebAPI
    /// </summary>
    public class AppDbContext : IdentityDbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public AppDbContext(DbContextOptions options) : base(options) {}

        /// <summary>
        /// Define la clave foránea compuesta de la entidad AuthorBook
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define la clave foránea compuesta de la entidad AuthorBook
            modelBuilder.Entity<AuthorBook>()
                .HasKey(ab => new { ab.AuthorId, ab.BookId });
        }


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

        /// <summary>
        /// Conexión con tabla de Comentarios
        /// </summary>
        public DbSet<AuthorBook>? AuthorsBooks { get; set; } = null!;
    }
}
