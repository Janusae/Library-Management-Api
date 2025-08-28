using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Book
{
    public class GetBookByIdCommand : IRequest<ServiceResponse<Domain.Sql.Entity.Book>>
    {
        public int Id { get; set; }
    }

    public class GetBookByIdHandler : IRequestHandler<GetBookByIdCommand, ServiceResponse<Domain.Sql.Entity.Book>>
    {
        private readonly ProgramDbContext _programDb;
        private readonly ResponseHandler _response;

        public GetBookByIdHandler(ProgramDbContext programDb, ResponseHandler response)
        {
            _programDb = programDb;
            _response = response;
        }

        public async Task<ServiceResponse<Domain.Sql.Entity.Book>> Handle(GetBookByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var book = await _programDb.Book
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsDeleted == false, cancellationToken);

                if (book == null)
                    return _response.CreateNotFound<Domain.Sql.Entity.Book>($"کتابی با شناسه {request.Id} یافت نشد");

                return _response.CreateSuccess("Book fetched successfully", book);
            }
            catch (Exception ex)
            {
                _response.CreateError<Domain.Sql.Entity.Book>("فراخوانی کتاب ناموفق بود", ex);
                throw new Application.Exceptions.AppException("فراخوانی کتاب ناموفق بود", "500");
            }
        }
    }
}
