using Microsoft.VisualBasic;
using WebApiAuthors.Entities;

namespace WebApiAuthors
{
    public class SeedData
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
                if (context.Authors.Any())
                {
                    return;   // DB has been seeded
                }

                // Autores /////////////////
                context.Authors.AddRange(
                    new Author()
                    {
                        Id = Guid.Parse("8032F738-4567-4503-945C-0D9CFBDF87FD"),
                        Name = "Anna Hackett"
                    },
                    new Author()
                    {
                        Id = Guid.Parse("D7F5451B-555F-4746-A6C2-0E584CF2D4FD"),
                        Name = "Eve Langelis"
                    },
                    new Author()
                    {
                        Id = Guid.Parse("EC1E5EF2-5F71-452B-A29A-399339AE59BE"),
                        Name = "Gena Showalter"
                    },
                    new Author()
                    {
                        Id = Guid.Parse("D01B3E5C-8AE4-4F74-8344-7087AD9F070D"),
                        Name = "Jeaniene Frost"
                    },
                    new Author()
                    {
                        Id = Guid.Parse("E6EDB099-37CA-4FEB-BB2C-856134E5D500"),
                        Name = "Johanna Lindsey"
                    },
                    new Author()
                    {
                        Id = Guid.Parse("E7AFFDAB-8F77-48CD-9D2F-9996F28CAD58"),
                        Name = "Kresley Cole"
                    },
                    new Author()
                    {
                        Id = Guid.Parse("CD9F706F-2C69-49A6-B38A-B4E34C5F4150"),
                        Name = "Sherrilyn Kenyon"
                    },
                    new Author()
                    {
                        Id = Guid.Parse("C99295EB-9167-4300-8071-E4318625C6DB"),
                        Name = "Suzanne Wright"
                    }
                );
                context.SaveChanges();


                //libros ////////////////////
                if (context.Books.Any())
                {
                    return;   // DB has been seeded
                }

                context.Books.AddRange(
                    new Book()
                    {
                        Id = Guid.Parse("589578A1-1469-47C9-83A8-0317A8593BE5"),
                        Title = "Here be sexist vampires"
                    },
                    new Book()
                    {
                        Id = Guid.Parse("5D04B355-682A-4D01-9C5C-A5FC2BFB4A07"),
                        Title = "Taste of torment"
                    },
                    new Book()
                    {
                        Id = Guid.Parse("1B697F1A-0474-4D9E-BE9E-DEF8C80AB841"),
                        Title = "The bite that binds"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Redemption"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Time Untime"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Styxx"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Acheron"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Dark Needs at Night"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Dark Desires After Dusk"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Pleasure of a Dark Prince"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Halfway To The Grave"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "One Foot in the Grave"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "At Grave's End"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Devil to Pay"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "The darkest touch"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Darkest Whisper"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Heart of darkness"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "B785"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "C791"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "F814"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Hades"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Wolf"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "The Powerbroker"
                    },
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Title = "The Specialist"
                    }
                );
                context.SaveChanges();

                //Comentarios ////////////////////
                if (context.Comments.Any())
                {
                    return;   // DB has been seeded
                }

                context.Comments.AddRange(
                    new Comment()
                    {
                        Id = Guid.NewGuid(),
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla tincidunt justo porta pellentesque ornare. Duis fringilla sed odio a hendrerit. Sed nunc sapien, lacinia ac mauris vitae, tincidunt tincidunt ex. Aenean vitae volutpat mi, in imperdiet ligula.",
                        BookID= Guid.Parse("589578A1-1469-47C9-83A8-0317A8593BE5"),
                        CreatedDate = DateTime.Now.AddDays(-21)
                    },
                    new Comment()
                    {
                        Id = Guid.NewGuid(),
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        BookID= Guid.Parse("589578A1-1469-47C9-83A8-0317A8593BE5"),
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
