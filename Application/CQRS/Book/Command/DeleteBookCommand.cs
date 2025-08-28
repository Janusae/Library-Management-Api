using MediatR;
using Application.DTO.BookDto;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Application.Exceptions;
using Application.Common;

namespace Application.CQRS.Book.Command
{
    public class DeleteBookCommand : IRequest<ServiceResponse<object>>
    {
        public string Id { get; set; }
    }

    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, ServiceResponse<object>>
    {
        private readonly ProgramDbContext _programDb;
        private readonly ResponseHandler _response;

        public DeleteBookCommandHandler(ProgramDbContext programDb, ResponseHandler response)
        {
            _programDb = programDb;
            _response = response;
        }

        public async Task<ServiceResponse<object>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!int.TryParse(request.Id, out int data))
                    return _response.CreateError<object>("Id is not valid!");

                var book = await _programDb.Book.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == data, cancellationToken);

                if (book is null)
                    return _response.CreateNotFound<object>("We could not find any book with your id!");

                _programDb.Book.Remove(book);
                await _programDb.SaveChangesAsync(cancellationToken);

                return _response.CreateSuccess<object>("کتاب با موفقیت حذف شد", null);
            }
            catch (DbUpdateException ex)
            {
                _response.CreateError<object>("خطای دیتابیس در حذف کتاب", ex);
                throw new AppException("خطای دیتابیس در حذف کتاب", "500");
            }
            catch (Exception ex)
            {
                _response.CreateError<object>("حذف کتاب ناموفق بود", ex);
                throw new AppException("حذف کتاب ناموفق بود", "500");
            }
        }
    }
}
