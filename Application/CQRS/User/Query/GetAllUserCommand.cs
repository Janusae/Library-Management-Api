using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.User
{
    public class GetAllUserCommand : IRequest<List<Domain.Sql.Entity.User>>
    {
    }

    public class GetAllUserHandler : IRequestHandler<GetAllUserCommand, List<Domain.Sql.Entity.User>>
    {
        private readonly ProgramDbContext _dbcontext;

        public GetAllUserHandler(ProgramDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<Domain.Sql.Entity.User>> Handle(GetAllUserCommand request, CancellationToken cancellationToken)
        {
            var users = await _dbcontext.Users
                .AsNoTracking()
                .Where(x => x.IsDeleted != true)
                .ToListAsync(cancellationToken);

            return users;
        }
    }
}