﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appDbContext"></param>
        /// <param name="mapper"></param>
        public CommentController(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
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
        /// Nuevo Comentario asociado a un Libro
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="commentAddDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post(Guid bookId, CommentAddDTO commentAddDTO)
        {
            var book = await _appDbContext.Books.FirstOrDefaultAsync(bookDB => bookDB.Id == bookId);
            if (book == null)
            {
                return NotFound();
            }

            var comment = _mapper.Map<Comment>(commentAddDTO);
            comment.BookID= bookId;
            _appDbContext.Add(comment);
            await _appDbContext.SaveChangesAsync();

            return Ok();
        }


    }
}