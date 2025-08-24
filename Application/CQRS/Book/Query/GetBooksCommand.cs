using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Book
{
    public class GetBooksCommand : IRequest<List<Domain.Sql.Entity.Book>>
    {
    }
    public class GetBooksHandler : IRequestHandler<GetBooksCommand, List<Domain.Sql.Entity.Book>>
    {
        public readonly ProgramDbContext _programDb;

        public GetBooksHandler(ProgramDbContext programDb)
        {
            _programDb = programDb;
        }

        public async Task<List<Domain.Sql.Entity.Book>> Handle(GetBooksCommand request, CancellationToken cancellationToken)
        {
            var books = await _programDb.Book.Where(x => x.IsDeleted == false).ToListAsync();
            return books;
        }
    }
}
