using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAuthors.DTOs;
using WebApiAuthors.Entities;

namespace WebApiAuthors.Controllers
{
    /// <summary>
    /// Gestión de Registros de Autores
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthorsController : ControllerBase
    {
        // Declaración variable DbContect
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<AuthorsController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appDbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public AuthorsController(AppDbContext appDbContext, 
            ILogger<AuthorsController> logger,
            IMapper mapper)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Todos los registros de la tabla Autores, Con DTO
        /// </summary>
        /// <returns>Listado de Autores, Con DTO</returns>
        /// /// <response code="401">Usuario No Autorizado</response>  
        [HttpGet] // api/authors
        [AllowAnonymous]
        [ProducesResponseType(401)]
        public async Task<ActionResult<List<AuthorDTO>>> Get()
        {
            var authors = await _appDbContext.Authors.ToListAsync();
            return _mapper.Map<List<AuthorDTO>>(authors);
        }

        /// <summary>
        /// Registro de Autor, dado su Id, con DTO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // Al añadir en la ruta la restrincción INT, al ingresar un tipo de datio no válido,
        // devolverá un error 404 (NotFaund), en lugar de 400(BadRequest).
        [HttpGet("{id:Guid}", Name ="getAuthorById")]// api/authors/1
        public async Task<ActionResult<AuthorDTOwithBooks>> Get(Guid id)
        {
            var author = await _appDbContext.Authors
                .Include(authorDB => authorDB.AuthorsBooks)
                .ThenInclude(authorBookDB => authorBookDB.Book)                
                .FirstOrDefaultAsync(authorDB => authorDB.Id == id);

            if (author == null)
            {
                return NotFound();
            }
            return _mapper.Map<AuthorDTOwithBooks>(author);
        }

        /// <summary>
        /// Registro de un Autor, dado parte o su Nombre completo, Con DTO
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        // No existe la resticción STRING. Si se especifica, devuelve error.
        [HttpGet("{name}")]// api/authors/name
        public async Task<ActionResult<List<AuthorDTO>>> Get([FromRoute] string name)
        {
            var authors = await _appDbContext.Authors.Where(authorDB => authorDB.Name.Contains(name)).ToListAsync();
            return _mapper.Map<List<AuthorDTO>>(authors);
        }

        /// <summary>
        /// Añadir nuevo Autor, Con DTO 
        /// </summary>
        /// <param name="authorAddDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post(AuthorAddDTO authorAddDTO)
        {
            // Validar si el nombre del autor ya existe en la DB.
            var authorExists = await _appDbContext.Authors.AnyAsync(authorDB => authorDB.Name == authorAddDTO.Name);

            if (authorExists)
            {
                return BadRequest($"El autor {authorAddDTO.Name}, ya existe en la BB - Controlador");
            }

            // Mapeado
            var author = _mapper.Map<Author>(authorAddDTO);

            // DB Query
            _appDbContext.Add(author);
            await _appDbContext.SaveChangesAsync();

            // Mapeado para la respuesta
            var authorDTO = _mapper.Map<AuthorDTO>(author);
            // Respuesta
            return CreatedAtRoute("getAuthorById", new { id = author.Id }, authorDTO);
        }

      
        /// <summary>
        /// Actualización Total de registro de Autor, dado su Id, Con DTO    
        /// </summary>
        /// <param name="authorAddDTO">AddDTO</param>
        /// <param name="id">Identificador del Comentario</param>
        /// <returns></returns>
        [HttpPut("{id:Guid}")]
        public async Task<ActionResult> Put(AuthorAddDTO authorAddDTO, Guid id)
        {
            // Validar si el nombre del autor ya existe en la DB.
            var authorExists = await _appDbContext.Authors.AnyAsync(authorDB => authorDB.Id == id);
            if (!authorExists)
            {
                // BadRequest --> Devuelve error 400
                return BadRequest($"El Autor con id {id}, no existe en la Base de Datos");
            }

            var author =_mapper.Map<Author>(authorAddDTO);
            author.Id = id;

            // Database
            _appDbContext.Update(author);
            await _appDbContext.SaveChangesAsync();
            return NoContent(); // Retorna 204
        }

        /// <summary>
        /// Eliminar registro de Autor, dado su Id, Sin DTO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var exists = await _appDbContext.Authors.AnyAsync(authorDB => authorDB.Id == id);
            if (!exists)
            {
                return NotFound($"El Autor con id {id}, no existe en la Base de Datos");
            }

            _appDbContext.Remove(new Author() { Id = id });
            await _appDbContext.SaveChangesAsync();

            return NoContent(); // Retorna 204

        }
    }
}
