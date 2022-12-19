using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace WebApiAuthors
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
           Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Servicios
        public void ConfigureServices(IServiceCollection services)
        {
            // Se alades las Opciones de Json para evitar los errores que se generar con las propiedades de navegación entre entidades.
            services.AddControllers().AddJsonOptions(x => 
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddDbContext<AppDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

           


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        // Middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            ILogger<Startup> logger)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
