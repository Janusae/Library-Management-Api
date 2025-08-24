using Application.CQRS.Book;
using Application.CQRS.Book.Command;
using Application.DTO.BookDto;
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
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetBooks()
        {
            var result = await _mediator.Send(new GetBooksCommand { });
            return Ok(result);
        }

        // GET: api/Book/{id}
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var result = await _mediator.Send(new GetBookByIdCommand{ Id = id});
            return Ok(result);
        }

        // POST: api/Book
        [HttpPost("Create")]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookDto request , CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateBookCommand { CreateBookDto = request });
            return Ok(result);
        }

        // PUT: api/Book/{id}
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateBook([FromBody] UpdateBookDto request)
        {
            var result = await _mediator.Send(new UpdateBookCommand { updateBookDto = request });
            return Ok(result);
        }

        // DELETE: api/Book/{id}
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            var result = await _mediator.Send(new DeleteBookCommand { Id = id });
            return Ok(result);
        }
    }
}
