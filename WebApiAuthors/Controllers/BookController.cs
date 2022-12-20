using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAuthors.DTOs;
using WebApiAuthors.Entities;

namespace WebApiAuthors.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        // Declaración variable DbContect
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;


        public BookController(AppDbContext appDbContext, IMapper mapper)
        {
            this._appDbContext = appDbContext;
            _mapper = mapper;
        }

        
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<BookDTO>> Get(Guid id)
        {
            var book = await _appDbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return _mapper.Map<BookDTO>(book);
        }

        [HttpPost]
        public async Task<ActionResult> Post(BookAddDTO BookAddDTO)
        {
            //var autjorExists = await _appDbContext.Authors.AnyAsync(x => x.Id == book.AuthorId);
            //if (!autjorExists)
            //{
            //    // BadRequest --> Devuelve error 400
            //    return BadRequest($"No existe un autor con Id: {book.AuthorId}");
            //}

            var book = _mapper.Map<Book>(BookAddDTO);    
            _appDbContext.Add(book);
            await _appDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
