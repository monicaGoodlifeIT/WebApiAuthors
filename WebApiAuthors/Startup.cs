using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
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
            services.AddControllers()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
                .AddNewtonsoftJson();

            services.AddDbContext<AppDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => 
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["secretKeyJwt"])), // Configurar la firma
                        ClockSkew = TimeSpan.Zero // Obtiene o establece el desfase horario que se aplicará al validar una hora.
                    });

            services.AddSwaggerGen(conf => {
                conf.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "WebApiAuthors",
                    Description = "Web API desarrollada con curso de UDEMY: Construyendo Web APIs RESTful con ASP.NET Core 6",
                    TermsOfService = new Uri("https://example.com/terms"),
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

                // Configuración Botón Autorize
                conf.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Token de Autorización"
                });
                // Funcionalidad Botón Autorize
                conf.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });

                // generate the XML docs that'll drive the swagger docs
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                conf.IncludeXmlComments(
                    xmlPath, includeControllerXmlComments: true);
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            

            // Automapper
            services.AddAutoMapper(typeof(Startup));

            // Identity
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
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

            // Authentication JWT
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
