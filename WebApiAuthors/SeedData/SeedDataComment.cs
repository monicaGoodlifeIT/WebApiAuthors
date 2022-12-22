using WebApiAuthors.Entities;

namespace WebApiAuthors.SeedData
{
    /// <summary>
    /// Sembrado Tabla Comentarios
    /// </summary>
    public class SeedDataComment
    {
        public static void Initialize(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                // Se comprueba si la DB ha sido creadra y existe.
                context.Database.EnsureCreated();

                // Comprueba si existen registros en la tabla correspondiente,
                // en cuyo caso, se sale del programa y no se siembra la DB.

                if (context.Comments.Any())
                {
                    return;   // DB has been seeded
                }

                context.Comments.AddRange(
                    new Comment()
                    {
                        Id = Guid.NewGuid(),
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla tincidunt justo porta pellentesque ornare. Duis fringilla sed odio a hendrerit. Sed nunc sapien, lacinia ac mauris vitae, tincidunt tincidunt ex. Aenean vitae volutpat mi, in imperdiet ligula.",
                        BookID = Guid.Parse("589578A1-1469-47C9-83A8-0317A8593BE5"),
                        CreatedDate = DateTime.Now.AddDays(-21)
                    },
                    new Comment()
                    {
                        Id = Guid.NewGuid(),
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        BookID = Guid.Parse("589578A1-1469-47C9-83A8-0317A8593BE5"),
                        CreatedDate = DateTime.Now.AddDays(-13)
                    },
                    new Comment()
                    {
                        Id = Guid.NewGuid(),
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla tincidunt justo porta pellentesque ornare. Duis fringilla sed odio a hendrerit. Sed nunc sapien, lacinia ac mauris vitae, tincidunt tincidunt ex.",
                        BookID = Guid.Parse("589578A1-1469-47C9-83A8-0317A8593BE5"),
                        CreatedDate = DateTime.Now.AddDays(-3)
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
