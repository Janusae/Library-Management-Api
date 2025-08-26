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

        public GetUserByIdHandler(ProgramDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<ServiceResponse<Domain.Sql.Entity.User>> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Id))
            {
                return ServiceResponse<Domain.Sql.Entity.User>.Error("شناسه کاربر نمی‌تواند خالی باشد.");
            }

            try
            {
                var user = await _dbcontext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(request.Id), cancellationToken);

                if (user == null)
                {
                    return ServiceResponse<Domain.Sql.Entity.User>.NotFound($"کاربری با شناسه '{request.Id}' یافت نشد.");
                }

                return ServiceResponse<Domain.Sql.Entity.User>.Success("User fetched successfully", user);
            }
            catch (Exception ex)
            {
                throw new AppException($"فراخوانی کاربر ناموفق بود: {ex.Message}", "500");
            }
        }
    }
}
