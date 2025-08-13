using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.User
{
    public class GetUserByIdCommand : IRequest<Domain.Sql.Entity.User>
    {
        public string Id { get; set; }
    }
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdCommand, Domain.Sql.Entity.User>
    {
        private readonly ProgramDbContext _dbcontext;

        public GetUserByIdHandler(ProgramDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Domain.Sql.Entity.User> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbcontext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.Id));
            return user;
        }
    }
}
