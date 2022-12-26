using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApiAuthors.DTOs;
using WebApiAuthors.Entities;

namespace WebApiAuthors.Controllers
{
    /// <summary>
    /// Controlador dependiente de BookController.
    /// La dependencia se establece en la ruta, que contendrá el Id del libro.
    /// </summary>
    [ApiController]
    [Route("api/[controller]/{bookId:Guid}/comments")]
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appDbContext">Contexto de la Aplicación</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="UserManager">Servicio de Identity</param>
        public CommentController(AppDbContext appDbContext
            ,IMapper mapper
            ,UserManager<IdentityUser> UserManager)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _userManager = UserManager;
        }

        /// <summary>
        /// Obtiene listado de Comentarios, dado el Id del Libro al que están asociados
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<CommentDTO>>> Get(Guid bookId)
        {
            var book = await _appDbContext.Books.FirstOrDefaultAsync(bookDB => bookDB.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }

            var comments = await _appDbContext.Comments
                .Where(commentDB => commentDB.BookID == bookId).ToListAsync();

            return _mapper.Map<List<CommentDTO>>(comments);
        }

        /// <summary>
        /// Obtiene data del comentrio, dado su Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:Guid}", Name = "GetCommentById")]
        public async Task<ActionResult<CommentDTO>> GetById(Guid id)
        {
            var comment = await _appDbContext.Comments.FirstOrDefaultAsync(commentDB => commentDB.Id == id);
            if (comment == null)
            {
                return NotFound();
            }                      

            return _mapper.Map<CommentDTO>(comment);
        }

        /// <summary>
        /// Nuevo Comentario asociado a un Libro
        /// </summary>
        /// <param name="bookId">Identificador del libro</param>
        /// <param name="commentAddDTO">DTO</param>
        /// <returns> Ruta del nuevo item creado</returns>
        /// <response code="201">la solicitud ha tenido éxito y ha llevado a la creación del recurso</response>  
        /// <response code="404">No se ha encontrado el item solicitado</response>  
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Post(Guid bookId, CommentAddDTO commentAddDTO)
        {
            // email tomado del token
            var emailClaim = HttpContext.User.Claims
                .Where(claim => claim.Type == "email")
                .FirstOrDefault();
            var email = emailClaim?.Value;
            var user = await _userManager.FindByEmailAsync(email);
            var userId = user.Id;

            // Libro al que se asocia el comentario  
            var book = await _appDbContext.Books.FirstOrDefaultAsync(bookDB => bookDB.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }

            var comment = _mapper.Map<Comment>(commentAddDTO);
            comment.BookID= bookId;
            comment.UserId = userId;
            _appDbContext.Add(comment);
            await _appDbContext.SaveChangesAsync();

            // Mapero de la respuesta
            var commentDTO = _mapper.Map<CommentDTO>(comment);
            return CreatedAtRoute("GetCommentById", new { id = comment.Id, bookId = bookId }, commentDTO);
        }

        /// <summary>
        /// Actualización TOTAL de registro de Comentario, dado su Id y el Id del libro, Con DTO 
        /// </summary>
        /// <param name="commentAddDTO">AddDTO</param>
        /// <param name="id">Identificador del Comentario</param>
        /// <param name="bookId">Identificador del Libro</param>
        /// <returns></returns>
        [HttpPut("{id:Guid}")]
        public async Task<ActionResult> Put(CommentAddDTO commentAddDTO, Guid id, Guid bookId)
        {
            var book = await _appDbContext.Books.AnyAsync(bookDB => bookDB.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }

            var commentExists = await _appDbContext.Comments.AnyAsync(commentDB => commentDB.Id == id);
            if(!commentExists)
            {
                return NotFound();
            }

            var comment = _mapper.Map<Comment>(commentAddDTO);
            comment.Id = id;
            comment.BookID = bookId;
            _appDbContext.Update(comment);
            await _appDbContext.SaveChangesAsync();

            return NoContent(); // Retorna 204
        }
    }
}
