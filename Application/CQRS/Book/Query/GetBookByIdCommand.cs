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

        public GetBookByIdHandler(ProgramDbContext programDb)
        {
            _programDb = programDb;
        }

        public async Task<ServiceResponse<Domain.Sql.Entity.Book>> Handle(GetBookByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var book = await _programDb.Book
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsDeleted == false, cancellationToken);

                if (book == null)
                    return ServiceResponse<Domain.Sql.Entity.Book>.NotFound($"کتابی با شناسه {request.Id} یافت نشد");

                return ServiceResponse<Domain.Sql.Entity.Book>.Success("Book fetched successfully", book);
            }
            catch (Exception ex)
            {
                throw new Application.Exceptions.AppException($"فراخوانی کتاب ناموفق بود: {ex.Message}", "500");
            }
        }
    }
}
