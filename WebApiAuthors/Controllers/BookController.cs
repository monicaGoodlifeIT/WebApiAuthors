using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
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
            AssignAuthorsOrder(book);
            // Envia a DB
            _appDbContext.Add(book);
            await _appDbContext.SaveChangesAsync();

            // Mapero de la respuesta
            var bookDTO = _mapper.Map<BookDTO>(book);
            return CreatedAtRoute("GetBookById", new { id = book.Id }, bookDTO);
        }

        /// <summary>
        /// Actualización TOTAL de Entidad Libro, incluyendo sus Autores. 
        /// Posteriormente se añadirán la Colección
        /// </summary>
        /// <param name="id">Identificador del Libro</param>
        /// <param name="bookAddDTO">AddDTO</param>
        /// <returns></returns>
        [HttpPut("{id:Guid}")]
        public async Task<ActionResult> Put(Guid id, BookAddDTO bookAddDTO)
        {
            var bookDB = await _appDbContext.Books
                .Include(x => x.AuthorsBooks)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (bookDB == null)
            {
                return NotFound(); // Retorna 400
            }
            // Mapeo que pasa la data de bookAddDTO a bookDB. Se guarda en libroBD,
            // lo que permite editar todos los valores del libro aprevechando
            // la instancia ya abierta con "var bookDB" y almacenada en memoria de Entity
            // Framework Core.
            bookDB = _mapper.Map(bookAddDTO, bookDB);

            // Orden de visualización de los autores
            AssignAuthorsOrder(bookDB);

            await _appDbContext.SaveChangesAsync();

            return NoContent(); // Retorna 204
        }

        private void AssignAuthorsOrder(Book book)
        {
            if (book.AuthorsBooks != null)
            {
                for (int i = 0; i < book.AuthorsBooks.Count; i++)
                {
                    book.AuthorsBooks[i].Order = i;
                }
            }
        }

        /// <summary>
        /// Actualización PARCIAL de Entidad Libro, incluyendo sus Autores. 
        /// </summary>
        /// <param name="id">Identificador del Libro</param>
        /// <param name="patchDocument">Documento JsonPatch</param>
        /// <returns></returns>
        [HttpPatch("{id:Guid}")]
        public async Task<ActionResult> Patch(Guid id, JsonPatchDocument<BookPatchDTO> patchDocument)
        {
            if(patchDocument == null)
            {
                return BadRequest("Formato incorrecto.");
            }

            var bookDB = await _appDbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (bookDB == null)
            {
                return NotFound($"El libro con id {id}, no existe en la Base de Datos."); // Retorna 400
            }

            var bookDTO = _mapper.Map<BookPatchDTO>(bookDB);

            // Aplicar los datos que vienen en el patchDocument, con mapeado
            // con bookDTO y el estado del modelo por si ocurren errores.
            patchDocument.ApplyTo(bookDTO, ModelState);

            // Validar los datos recibidos
            var isValid = TryValidateModel(bookDTO);
            if (!isValid)
            {
                return BadRequest(ModelState); // Muestra errores de validación
            }

            _mapper.Map(bookDTO, bookDB);
            await _appDbContext.SaveChangesAsync();

            return NoContent(); // Retorna 204
        }

        /// <summary>
        /// Eliminar registro de Libro y sus dependenicas, dado su Id, Sin DTO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var exists = await _appDbContext.Books.AnyAsync(bookDB => bookDB.Id == id);
            if (!exists)
            {
                return NotFound($"El Libro con id {id}, no existe en la Base de Datos");
            }

            _appDbContext.Remove(new Book() { Id = id });
            await _appDbContext.SaveChangesAsync();

            return NoContent(); // Retorna 204

        }
    }
}
