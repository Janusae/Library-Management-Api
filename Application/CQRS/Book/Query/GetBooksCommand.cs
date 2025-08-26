using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Book
{
    public class GetBooksCommand : IRequest<ServiceResponse<List<Domain.Sql.Entity.Book>>>
    {
    }

    public class GetBooksHandler : IRequestHandler<GetBooksCommand, ServiceResponse<List<Domain.Sql.Entity.Book>>>
    {
        private readonly ProgramDbContext _programDb;

        public GetBooksHandler(ProgramDbContext programDb)
        {
            _programDb = programDb;
        }

        public async Task<ServiceResponse<List<Domain.Sql.Entity.Book>>> Handle(GetBooksCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var books = await _programDb.Book
                    .AsNoTracking()
                    .Where(x => x.IsDeleted == false)
                    .ToListAsync(cancellationToken);

                return ServiceResponse<List<Domain.Sql.Entity.Book>>.Success("Books fetched successfully", books);
            }
            catch (Exception ex)
            {
                throw new Application.Exceptions.AppException($"فراخوانی کتاب‌ها ناموفق بود: {ex.Message}", "500");
            }
        }
    }
}
