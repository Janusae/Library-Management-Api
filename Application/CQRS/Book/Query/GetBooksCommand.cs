using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common;

namespace Application.CQRS.Book
{
    public class GetBooksCommand : IRequest<ServiceResponse<List<Domain.Sql.Entity.Book>>>
    {
    }

    public class GetBooksHandler : IRequestHandler<GetBooksCommand, ServiceResponse<List<Domain.Sql.Entity.Book>>>
    {
        private readonly ProgramDbContext _programDb;
        private readonly ResponseHandler _response;

        public GetBooksHandler(ProgramDbContext programDb, ResponseHandler response)
        {
            _programDb = programDb;
            _response = response;
        }

        public async Task<ServiceResponse<List<Domain.Sql.Entity.Book>>> Handle(GetBooksCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var books = await _programDb.Book
                    .AsNoTracking()
                    .Where(x => x.IsDeleted == false)
                    .ToListAsync(cancellationToken);

                return _response.CreateSuccess("Books fetched successfully", books);
            }
            catch (Exception ex)
            {
                _response.CreateError<List<Domain.Sql.Entity.Book>>("فراخوانی کتاب‌ها ناموفق بود", ex);
                throw new Application.Exceptions.AppException("فراخوانی کتاب‌ها ناموفق بود", "500");
            }
        }
    }
}
