using WebApiAuthors.Entities;

namespace WebApiAuthors.SeedData
{
    public class SeedDataBookCollection
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
                if (context.BookCollections.Any())
                {
                    return;   // DB has been seeded
                }

                context.BookCollections.AddRange(
                    new BookCollection()
                    {
                        Id = Guid.Parse("6640F0BE-D81D-4E2B-8A26-00C0F8CC85FD"),
                        Title = "Deep in your veins",
                        Genre = GenreType.ScienceFiction,
                        RegisterCreated = DateTime.Now.AddDays(-21)
                    },
                    new BookCollection()
                    {
                        Id = Guid.Parse("907FB730-541B-4334-8C21-42969CCDD942"),
                        Title = "Cyborgs, more than machines",
                        Genre = GenreType.ScienceFiction,
                        RegisterCreated = DateTime.Now.AddDays(-11)
                    },
                    new BookCollection()
                    {
                        Id = Guid.Parse("40F4ABE5-41F0-4A69-8B21-4C622FA4E5C6"),
                        Title = "Dark Hunter",
                        Genre = GenreType.Fantasy,
                        RegisterCreated = DateTime.Now.AddDays(-29)
                    },
                    new BookCollection()
                    {
                        Id = Guid.Parse("0C1C7DA3-FD61-4670-8CD4-8D4E149E5037"),
                        Title = "Immortals After Dark",
                        Genre = GenreType.Fantasy,
                        RegisterCreated = DateTime.Now.AddDays(-32)
                    },
                    new BookCollection()
                    {
                        Id = Guid.Parse("E1C16174-43CC-427F-9C6C-8FD0DD4DBF1A"),
                        Title = "Lords of the Underworld",
                        Genre = GenreType.Fantasy,
                        RegisterCreated = DateTime.Now.AddDays(-33)
                    },
                    new BookCollection()
                    {
                        Id = Guid.Parse("EB1DE9EF-E5AF-416D-A056-B3607808D9BB"),
                        Title = "Malory",
                        Genre = GenreType.History,
                        RegisterCreated = DateTime.Now.AddDays(-39)
                    },
                    new BookCollection()
                    {
                        Id = Guid.Parse("AA0BAA84-FCB6-49BD-A98A-8AA116516212"),
                        Title = "Night Huntress",
                        Genre = GenreType.History,
                        RegisterCreated = DateTime.Now.AddDays(-39)
                    },
                    new BookCollection()
                    {
                        Id = Guid.Parse("2F70AB2D-5E86-440A-B9A9-5D06F0CDCE4A"),
                        Title = "Sentinel Security",
                        Genre = GenreType.History,
                        RegisterCreated = DateTime.Now.AddDays(-39)
                    },
                    new BookCollection()
                    {
                        Id = Guid.Parse("523C5890-6787-4AC1-8E34-18AFFA797120"),
                        Title = "Norcross",
                        Genre = GenreType.History,
                        RegisterCreated = DateTime.Now.AddDays(-39)
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
