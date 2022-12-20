using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
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
            // Se añaden las Opciones de Json para evitar los errores que se generan con las propiedades de navegación entre entidades.
            services.AddControllers().AddJsonOptions(x => 
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddDbContext<AppDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(conf => { 
                conf.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "WebApiAuthors",
                    Description = "Web API desarrollada con curso de UDEMY: Construyendo Web APIs RESTful con ASP.NET Core 6",
                    Contact = new OpenApiContact
                    {
                        Name = "Mónica Buenavida Corono",
                        Email = "monicabuenavida@outlook.com",
                        Url = new Uri("https://github.com/monicaGoodlifeIT/WebApiAuthors")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "UDEMY: Construyendo Web APIs RESTful con ASP.NET Core 6",
                        Url = new Uri("https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/13300602#overview")
                    },
                    Version = "v1"
                });


                 // generate the XML docs that'll drive the swagger docs
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                conf.IncludeXmlComments(
                    xmlPath, includeControllerXmlComments: true);
            });

            // Automapper
            services.AddAutoMapper(typeof(Startup));
        }

        // Middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            ILogger<Startup> logger)
        {

            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web APIs RESTful con ASP.NET Core 6 - V1");
                });
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
