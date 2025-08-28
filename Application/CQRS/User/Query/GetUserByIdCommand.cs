using Application.Common;
using Application.Exceptions;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.User
{
    public class GetUserByIdCommand : IRequest<ServiceResponse<Domain.Sql.Entity.User>>
    {
        public string Id { get; set; }
    }

    public class GetUserByIdHandler : IRequestHandler<GetUserByIdCommand, ServiceResponse<Domain.Sql.Entity.User>>
    {
        private readonly ProgramDbContext _dbcontext;
        private readonly ResponseHandler _response;

        public GetUserByIdHandler(ProgramDbContext dbcontext, ResponseHandler response)
        {
            _dbcontext = dbcontext;
            _response = response;
        }

        public async Task<ServiceResponse<Domain.Sql.Entity.User>> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Id))
            {
                return _response.CreateError<Domain.Sql.Entity.User>("شناسه کاربر نمی‌تواند خالی باشد.");
            }

            try
            {
                var user = await _dbcontext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(request.Id), cancellationToken);

                if (user == null)
                {
                    return _response.CreateNotFound<Domain.Sql.Entity.User>($"کاربری با شناسه '{request.Id}' یافت نشد.");
                }

                return _response.CreateSuccess("User fetched successfully", user);
            }
            catch (Exception ex)
            {
                _response.CreateError<Domain.Sql.Entity.User>("فراخوانی کاربر ناموفق بود", ex);
                throw new AppException("فراخوانی کاربر ناموفق بود", "500");
            }
        }
    }
}
