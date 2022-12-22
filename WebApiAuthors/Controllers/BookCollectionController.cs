using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAuthors.DTOs;
using WebApiAuthors.Entities;

namespace WebApiAuthors.Controllers
{
    /// <summary>
    /// Gestión de Registros de Series de Libros
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BookCollectionController : ControllerBase
    {
        /// <summary>
        /// Declaración variables privadas
        /// </summary>
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<BookCollectionController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appDbContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public BookCollectionController(AppDbContext appDbContext
            ,ILogger<BookCollectionController> logger
            ,IMapper mapper)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Listado de Colecciones, Con DTO
        /// </summary>
        /// <returns></returns>
        [HttpGet] // api/bookcollection
        public async Task<ActionResult<List<BookCollectionDTO>>> Get()
        {
            var bookCollection = await _appDbContext.BookCollections
                .Include(BookCollectionsDB => BookCollectionsDB.Books)
                .ToListAsync();
            return _mapper.Map<List<BookCollectionDTO>>(bookCollection);
        }

        /// <summary>
        /// Registro de Coleeción, dado su Id, con DTO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:Guid}", Name ="GetBookCollectionById")]// api/bookcollection/8032F738-4567-4503-945C-0D9CFBDF87FD
        public async Task<ActionResult<BookCollectionDTO>> Get(Guid id)
        {
            var bookCollection = await _appDbContext.BookCollections
                .FirstOrDefaultAsync(bookCollectionDB => bookCollectionDB.Id == id);

            if (bookCollection == null)
            {
                return NotFound();
            }
            return _mapper.Map<BookCollectionDTO>(bookCollection);
        }


        /// <summary>
        /// Añadir nueva Colección, con DTO
        /// </summary>
        /// <param name="bookCollectionAddDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post(BookCollectionAddDTO bookCollectionAddDTO)
        {
            // Validar si el nombre del autor ya existe en la DB.
            var bookCollectionExists = await _appDbContext.BookCollections
                .AnyAsync(bookCollectionDB => bookCollectionDB.Title == bookCollectionAddDTO.Title);

            if (bookCollectionExists)
            {
                return BadRequest($"La Collección '{bookCollectionAddDTO.Title}', ya existe en la BB - Controlador");
            }

            // Mapeado
            var bookCollection = _mapper.Map<BookCollection>(bookCollectionAddDTO);

            // DB Query
            _appDbContext.Add(bookCollection);
            await _appDbContext.SaveChangesAsync();

            // Mapeado de respuesta
            var bookCollectionDTO = _mapper.Map<BookCollectionDTO>(bookCollection);
            return CreatedAtRoute("GetBookCollectionById", new { id = bookCollection.Id }, bookCollectionDTO);
        }
    }
}
