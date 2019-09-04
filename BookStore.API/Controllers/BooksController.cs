using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly DataContext _context;

        public BooksController(DataContext context)
        {
            _context = context;
        }
        // GET api/Books
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var books = await _context.Books.ToListAsync();
            return Ok(books);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(book);
        }
    }
}