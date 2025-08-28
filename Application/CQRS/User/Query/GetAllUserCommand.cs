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
        private readonly ResponseHandler _response;

        public GetAllUserHandler(ProgramDbContext dbcontext, ResponseHandler response)
        {
            _dbcontext = dbcontext;
            _response = response;
        }

        public async Task<ServiceResponse<System.Collections.Generic.List<Domain.Sql.Entity.User>>> Handle(GetAllUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _dbcontext.Users
                    .AsNoTracking()
                    .Where(x => x.IsDeleted != true)
                    .ToListAsync(cancellationToken);

                return _response.CreateSuccess("Users fetched successfully", users);
            }
            catch (Exception ex)
            {
                _response.CreateError<List<Domain.Sql.Entity.User>>("فراخوانی کاربران ناموفق بود", ex);

                throw new AppException("فراخوانی کاربران ناموفق بود", "500");
            }
        }
    }
}
