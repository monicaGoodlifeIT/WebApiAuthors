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
        [HttpGet("{id:Guid}", Name ="GetBookById")]
        public async Task<ActionResult<BookDTOwithAuthors>> Get(Guid id)
        {
            var book = await _appDbContext.Books.Include(bookDB => bookDB.AuthorsBooks)
                .ThenInclude(authorBookDB => authorBookDB.Author)
                .Include(bookDB => bookDB.Comments)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (book == null)
            {
                return NotFound();
            }
            // Orden de visualización de los autores
            book.AuthorsBooks = book.AuthorsBooks.OrderBy(x => x.Order).ToList();

            return _mapper.Map<BookDTOwithAuthors>(book);
        }

 
        /// <summary>
        /// Añade registro de Libro, con DTO
        /// </summary>
        /// <param name="bookAddDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post(BookAddDTO bookAddDTO)
        {
            // Validar la existencia de autores
            if (bookAddDTO.AuthorsIds == null)
            {
                return BadRequest("No se pued añadir un Libro si no existen Autores");
            }

            // Validar existencia de todos los autores recibidos
            var authorsIds = await _appDbContext.Authors
                .Where(authorDB => bookAddDTO.AuthorsIds.Contains(authorDB.Id))
                .Select(authorDB => authorDB.Id)
                .ToListAsync();
            if(bookAddDTO.AuthorsIds.Count() != authorsIds.Count())
            {
                return BadRequest("Alguno de los autores enviados, no existe");
            }

            // Mapeo
            var book = _mapper.Map<Book>(bookAddDTO);  
            
            // Orden de visualización de los autores
            if(book.AuthorsBooks != null)
            {
                for(int i = 0; i < book.AuthorsBooks.Count; i++)
                {
                    book.AuthorsBooks[i].Order= i;
                }
            }
            // Envia a DB
            _appDbContext.Add(book);
            await _appDbContext.SaveChangesAsync();

            // Mapero de la respuesta
            var bookDTO = _mapper.Map<BookDTO>(book);
            return CreatedAtRoute("GetBookById", new { id = book.Id }, bookDTO);
        }
    }
}
