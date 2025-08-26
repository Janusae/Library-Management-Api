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

        public DeleteBookCommandHandler(ProgramDbContext programDb)
        {
            _programDb = programDb;
        }

        public async Task<ServiceResponse<object>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!int.TryParse(request.Id, out int data))
                    return ServiceResponse<object>.Error("Id is not valid!");

                var book = await _programDb.Book.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == data, cancellationToken);

                if (book is null)
                    return ServiceResponse<object>.NotFound("We could not find any book with your id!");

                _programDb.Book.Remove(book);
                await _programDb.SaveChangesAsync(cancellationToken);

                return ServiceResponse<object>.Success("کتاب با موفقیت حذف شد", null);
            }
            catch (DbUpdateException ex)
            {
                throw new AppException("خطای دیتابیس در حذف کتاب", "500");
            }
            catch (Exception ex)
            {
                throw new AppException("حذف کتاب ناموفق بود", "500");
            }
        }
    }
}
