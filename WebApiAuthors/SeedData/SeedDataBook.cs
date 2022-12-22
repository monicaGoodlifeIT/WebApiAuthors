using WebApiAuthors.Entities;

namespace WebApiAuthors.SeedData
{
    public class SeedDataBook
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
                if (context.Books.Any())
                {
                    return;   // DB has been seeded
                }

                context.Books.AddRange(
                    new Book()
                    {
                        Id = Guid.Parse("589578A1-1469-47C9-83A8-0317A8593BE5"),
                        Title = "Here be sexist vampires",
                        BookCollectionID = Guid.Parse("6640F0BE-D81D-4E2B-8A26-00C0F8CC85FD"),
                        Order = 1,
                    },
                    new Book()
                    {
                        Id = Guid.Parse("5D04B355-682A-4D01-9C5C-A5FC2BFB4A07"),
                        Title = "Taste of torment",
                        BookCollectionID = Guid.Parse("6640F0BE-D81D-4E2B-8A26-00C0F8CC85FD"),
                        Order = 3
                    },
                    new Book()
                    {
                        Id = Guid.Parse("1B697F1A-0474-4D9E-BE9E-DEF8C80AB841"),
                        Title = "The bite that binds",
                        BookCollectionID = Guid.Parse("6640F0BE-D81D-4E2B-8A26-00C0F8CC85FD"),
                        Order = 2
                    },
                    new Book()
                    {
                        Id = Guid.Parse("474889EC-F886-465A-9AED-043CAD75BF9E"),
                        Title = "The Guardian",
                        BookCollectionID = Guid.Parse("40F4ABE5-41F0-4A69-8B21-4C622FA4E5C6"),
                        Order = 31
                    },
                    new Book()
                    {
                        Id = Guid.Parse("E72C3DDB-BE04-42DD-B944-0C7385A0A317"),
                        Title = "Time Untime",
                        BookCollectionID = Guid.Parse("40F4ABE5-41F0-4A69-8B21-4C622FA4E5C6"),
                        Order = 32
                    },
                    new Book()
                    {
                        Id = Guid.Parse("57ED07B8-7C79-4ABC-B2D9-0DEA4A27C92E"),
                        Title = "Styxx",
                        BookCollectionID = Guid.Parse("40F4ABE5-41F0-4A69-8B21-4C622FA4E5C6"),
                        Order = 33
                    },
                    new Book()
                    {
                        Id = Guid.Parse("8845B865-4762-4B83-ABA7-2841C8B239A1"),
                        Title = "Acheron",
                        BookCollectionID = Guid.Parse("40F4ABE5-41F0-4A69-8B21-4C622FA4E5C6"),
                        Order = 28
                    },
                    new Book()
                    {
                        Id = Guid.Parse("E75BF883-5416-4AE4-B62D-465B8956538F"),
                        Title = "Dark Needs at Night",
                        BookCollectionID = Guid.Parse("0C1C7DA3-FD61-4670-8CD4-8D4E149E5037"),
                        Order = 5

                    },
                    new Book()
                    {
                        Id = Guid.Parse("7944E6CA-9129-4FDC-A5B9-4F55169542A8"),
                        Title = "Dark Desires After Dusk",
                        BookCollectionID = Guid.Parse("0C1C7DA3-FD61-4670-8CD4-8D4E149E5037"),
                        Order = 6
                    },
                    new Book()
                    {
                        Id = Guid.Parse("5B16EAEE-5733-484F-B03A-5DC9085394D2"),
                        Title = "Pleasure of a Dark Prince",
                        BookCollectionID = Guid.Parse("0C1C7DA3-FD61-4670-8CD4-8D4E149E5037"),
                        Order = 8
                    },
                    new Book()
                    {
                        Id = Guid.Parse("6274D8F8-B796-4A00-A1FC-6392CF0C76CA"),
                        Title = "Halfway To The Grave",
                        BookCollectionID = Guid.Parse("AA0BAA84-FCB6-49BD-A98A-8AA116516212"),
                        Order = 2
                    },
                    new Book()
                    {
                        Id = Guid.Parse("F9373B01-3397-423A-8F6E-6F0BBDCF9F62"),
                        Title = "One Foot in the Grave",
                        BookCollectionID = Guid.Parse("AA0BAA84-FCB6-49BD-A98A-8AA116516212"),
                        Order = 3
                    },
                    new Book()
                    {
                        Id = Guid.Parse("A5E5E020-5617-4071-8C9C-821A7E8A4D9B"),
                        Title = "At Grave's End",
                        BookCollectionID = Guid.Parse("AA0BAA84-FCB6-49BD-A98A-8AA116516212"),
                        Order = 5
                    },
                    new Book()
                    {
                        Id = Guid.Parse("B5E4D4C6-B511-45F0-9BF9-B70BBCA2549D"),
                        Title = "Devil to Pay",
                        BookCollectionID = Guid.Parse("AA0BAA84-FCB6-49BD-A98A-8AA116516212"),
                        Order = 6
                    },
                    new Book()
                    {
                        Id = Guid.Parse("ECEF578D-9CB2-4D8C-B57D-C44FE4081F51"),
                        Title = "The darkest touch",
                        BookCollectionID = Guid.Parse("E1C16174-43CC-427F-9C6C-8FD0DD4DBF1A"),
                        Order = 12
                    },
                    new Book()
                    {
                        Id = Guid.Parse("7806F2C4-E3B1-4314-8D81-CBD03E3986DE"),
                        Title = "Darkest Whisper",
                        BookCollectionID = Guid.Parse("E1C16174-43CC-427F-9C6C-8FD0DD4DBF1A"),
                        Order = 5
                    },
                    new Book()
                    {
                        Id = Guid.Parse("BE878AAC-ED11-442D-90F4-D65B5EBC2E9F"),
                        Title = "Heart of darkness",
                        BookCollectionID = Guid.Parse("E1C16174-43CC-427F-9C6C-8FD0DD4DBF1A"),
                        Order = 4
                    },
                    new Book()
                    {
                        Id = Guid.Parse("CE3DD479-1322-42C7-8AE6-DA5797E7F9D4"),
                        Title = "B785",
                        BookCollectionID = Guid.Parse("907FB730-541B-4334-8C21-42969CCDD942"),
                        Order = 3
                    },
                    new Book()
                    {
                        Id = Guid.Parse("B1A8D126-44C5-47B2-949B-DDB6158AB8A9"),
                        Title = "C791",
                        BookCollectionID = Guid.Parse("907FB730-541B-4334-8C21-42969CCDD942"),
                        Order = 1
                    },
                    new Book()
                    {
                        Id = Guid.Parse("966D5269-B654-43FE-AF62-EFF556FF0369"),
                        Title = "F814",
                        BookCollectionID = Guid.Parse("907FB730-541B-4334-8C21-42969CCDD942"),
                        Order = 2
                    },
                    new Book()
                    {
                        Id = Guid.Parse("C853BC17-C39D-47B2-963E-E37C35B414A3"),
                        Title = "Hades",
                        BookCollectionID = Guid.Parse("2F70AB2D-5E86-440A-B9A9-5D06F0CDCE4A"),
                        Order = 2
                    },
                    new Book()
                    {
                        Id = Guid.Parse("5E5FAEE4-8CE3-4F1D-A3CA-E7A883825AC1"),
                        Title = "Wolf",
                        BookCollectionID = Guid.Parse("2F70AB2D-5E86-440A-B9A9-5D06F0CDCE4A"),
                        Order = 1
                    },
                    new Book()
                    {
                        Id = Guid.Parse("36FFD952-194A-47A8-83BA-EC496C14567D"),
                        Title = "The Powerbroker",
                        BookCollectionID = Guid.Parse("523C5890-6787-4AC1-8E34-18AFFA797120"),
                        Order = 6
                    },
                    new Book()
                    {
                        Id = Guid.Parse("420C6DE5-1949-4188-AC5D-ED3ABBF9ACCD"),
                        Title = "The Specialist",
                        BookCollectionID = Guid.Parse("523C5890-6787-4AC1-8E34-18AFFA797120"),
                        Order = 3
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
