using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAuthors.DTOs;
using WebApiAuthors.Entities;

namespace WebApiAuthors.Controllers
{
    /// <summary>
    /// Gestión de Registros de Libros
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    //[Route("api/[controller]/{BookCollectionID:Guid}")]
    public class BookController : ControllerBase
    {
        // Declaración variable DbContect
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="appDbContext"></param>
        /// <param name="mapper"></param>
        public BookController(AppDbContext appDbContext, IMapper mapper)
        {
            this._appDbContext = appDbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Listado de Colecciones, Con DTO
        /// </summary>
        /// <returns></returns>
        [HttpGet] // api/bookcollection
        public async Task<ActionResult<List<BookDTO>>> Get()
        {
            var books = await _appDbContext.Books
                .Include(bookDB => bookDB.Comments)
                .ToListAsync();
            return _mapper.Map<List<BookDTO>>(books);
        }

        /// <summary>
        /// Registro de un Libro, dado su Id, con DTO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<BookDTO>> Get(Guid id)
        {
            var book = await _appDbContext.Books
                .Include(bookDB => bookDB.Comments)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return _mapper.Map<BookDTO>(book);
        }

        /// <summary>
        /// Añade registro de Libro, con DTO
        /// </summary>
        /// <param name="BookAddDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post(BookAddDTO BookAddDTO)
        {
            var book = _mapper.Map<Book>(BookAddDTO);    
            _appDbContext.Add(book);
            await _appDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
