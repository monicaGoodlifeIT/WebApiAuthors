using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAuthors.Entities;

namespace WebApiAuthors.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        // Declaración variable DbContect
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(AppDbContext appDbContext, ILogger<AuthorsController> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        // Acción con varias rutas
        [HttpGet] // api/authors
        [HttpGet("authorslist")] // api/authors/authorslist
        [HttpGet("/authorslist")] // authorslist
        public async Task<ActionResult<List<Author>>> Get()
        {
            //return new List<Author>()
            //{
            //    new Author(){Id=1, Name="Felipe"},
            //    new Author(){Id=2, Name="Claudio"},
            //};
            _logger.LogInformation("Listado de Autores");

            return await _appDbContext.Authors.Include(x => x.Books).ToListAsync();
        }

        [HttpGet("author")] // api/authors/author
        public async Task<ActionResult<Author>> FirstAuthor()
        {
            return await _appDbContext.Authors.FirstOrDefaultAsync();
        }

        // Al añadir en la ruta la restrincción INT, al ingresar un tipo de datio no válido,
        // devolverá un error 404 (NotFaund), en lugar de 400(BadRequest).
        [HttpGet("{id:int}")]// api/authors/1
        public async Task<ActionResult<Author>> Get(int id)
        {
            var author = await _appDbContext.Authors.FirstOrDefaultAsync(x => x.Id == id);

            if (author == null)
            {
                return NotFound();
            }
            return author;
        }

        // No existe la resticción STRING. Si se especifica, devuelve error.
        [HttpGet("{name}")]// api/authors/name
        [HttpGet("{name}/{parameter2?}")]// api/authors/name/parameter2 --> el parametro2 es opcional
        [HttpGet("{name}/{parameter2 = persona}")]// api/authors/name/persona --> el parametro2 tiene valor por defecto
        public async Task<ActionResult<Author>> Get(string name, string parameter2)
        {
            var author = await _appDbContext.Authors.FirstOrDefaultAsync(x => x.Name.Contains(name));

            if (author == null)
            {
                return NotFound();
            }
            return author;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Author author)
        {
            // Validar si el nombre del autor ya existe en la DB.
            var authorExists = await _appDbContext.Authors.AnyAsync(x =>x.Name == author.Name);

            if (authorExists)
            {
                return BadRequest($"El autor {author.Name }, ya existe en la BB - Controlador");
            }
             
            _appDbContext.Add(author);
            await _appDbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Author author, int id)
        {
            if(author.Id != id)
            {
                // BadRequest --> Devuelve error 400
                return BadRequest("El id del autor no coincide con el id de la URL");
            }

            _appDbContext.Update(author);
            await _appDbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await _appDbContext.Authors.AnyAsync(author => author.Id == id);
            if (!exists)
            {
                return NotFound();
            }
       
            _appDbContext.Remove(new Author() { Id = id });
            await _appDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
