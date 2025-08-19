using Application.Exceptions;
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
            if (string.IsNullOrWhiteSpace(request.Id))
            {
                throw new AppException("شناسه کاربر نمی‌تواند خالی باشد.", "400");
            }

            try
            {
                var user = await _dbcontext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(request.Id), cancellationToken);

                if (user == null)
                {
                    throw new AppException($"کاربری با شناسه '{request.Id}' یافت نشد.", "404");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new AppException($"فراخوانی کاربر ناموفق بود: {ex.Message}", "500");
            }

        }
    }
}
