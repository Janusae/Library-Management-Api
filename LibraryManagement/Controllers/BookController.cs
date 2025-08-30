using Application.CQRS.Book;
using Application.CQRS.Book.Command;
using Application.DTO.BookDto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetBooks()
        {
            var result = await _mediator.Send(new GetBooksCommand { });
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var result = await _mediator.Send(new GetBookByIdCommand{ Id = id});
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookDto request , CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateBookCommand { CreateBookDto = request });
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateBook([FromBody] UpdateBookDto request)
        {
            var result = await _mediator.Send(new UpdateBookCommand { updateBookDto = request });
            return Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            var result = await _mediator.Send(new DeleteBookCommand { Id = id });
            return Ok(result);
        }
    }
}
