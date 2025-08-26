using Application.Common;
using Application.Exceptions;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.User
{
    public class GetAllUserCommand : IRequest<ServiceResponse<System.Collections.Generic.List<Domain.Sql.Entity.User>>>
    {
    }

    public class GetAllUserHandler : IRequestHandler<GetAllUserCommand, ServiceResponse<System.Collections.Generic.List<Domain.Sql.Entity.User>>>
    {
        private readonly ProgramDbContext _dbcontext;

        public GetAllUserHandler(ProgramDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<ServiceResponse<System.Collections.Generic.List<Domain.Sql.Entity.User>>> Handle(GetAllUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _dbcontext.Users
                    .AsNoTracking()
                    .Where(x => x.IsDeleted != true)
                    .ToListAsync(cancellationToken);

                return ServiceResponse<System.Collections.Generic.List<Domain.Sql.Entity.User>>.Success("Users fetched successfully", users);
            }
            catch (Exception ex)
            {
                throw new AppException("فراخوانی کاربران ناموفق بود", "500");
            }
        }
    }
}
