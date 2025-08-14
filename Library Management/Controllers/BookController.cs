using Application.CQRS.Book.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Book
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var result = await _mediator.Send(new GetBooksCommand { });
            return Ok(result);
        }

        // GET: api/Book/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var result = await _mediator.Send(new GetBookByIdCommand{ Id = id});
            return Ok(result);
        }

        // POST: api/Book
        [HttpPost]
        public IActionResult CreateBook([FromBody] object book)
        {
            return Ok();
        }

        // PUT: api/Book/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] object book)
        {
            return Ok();
        }

        // DELETE: api/Book/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            return Ok();
        }
    }
}
