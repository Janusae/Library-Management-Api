using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Book
{
    public class GetBookByIdCommand : IRequest<Domain.Sql.Entity.Book>
    {
        public int Id { get; set; }
    }

    public class GetBookByIdHandler : IRequestHandler<GetBookByIdCommand, Domain.Sql.Entity.Book>
    {
        private readonly ProgramDbContext _programDb;

        public GetBookByIdHandler(ProgramDbContext programDb)
        {
            _programDb = programDb;
        }

        public async Task<Domain.Sql.Entity.Book> Handle(GetBookByIdCommand request, CancellationToken cancellationToken)
        {
            var book = await _programDb.Book
                .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

            return book;
        }
    }
}
