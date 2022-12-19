using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAuthors.Entities;

namespace WebApiAuthors.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        // Declaración variable DbContect
        private readonly AppDbContext _appDbContext;

        public BookController(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }


        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<Book>> Get(int id)
        //{
        //    return await _appDbContext.Books.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id == id);
        //}

        //[HttpPost]
        //public async Task<ActionResult> Post(Book book)
        //{
        //    var autjorExists = await _appDbContext.Authors.AnyAsync(x => x.Id == book.AuthorId);
        //    if (!autjorExists)
        //    {
        //        // BadRequest --> Devuelve error 400
        //        return BadRequest($"No existe un autor con Id: {book.AuthorId }");
        //    }
            
        //    _appDbContext.Add(book);
        //    await _appDbContext.SaveChangesAsync();
        //    return Ok();
        //}
    }
}
