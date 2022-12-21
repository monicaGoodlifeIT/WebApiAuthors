using WebApiAuthors;
//using WebApiAuthors.SeedData;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Seed the database
//SeedData.Initialize(app);
//SeedDataAuthor.Initialize(app);
//SeedDataBookCollection.Initialize(app);
//SeedDataBook.Initialize(app);
//SeedDataComment.Initialize(app);

var loggerService = (ILogger<Startup>?)app.Services.GetService(typeof(ILogger<Startup>));

startup.Configure(app, app.Environment, loggerService);

app.Run();
