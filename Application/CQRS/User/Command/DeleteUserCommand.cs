using Application.Common;
using Application.Exceptions;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.User
{
    public class DeleteUserCommand : IRequest<ServiceResponse<object>>
    {
        public string Id { get; set; }
    }

    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, ServiceResponse<object>>
    {
        private readonly ProgramDbContext _dbcontext;

        public DeleteUserHandler(ProgramDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<ServiceResponse<object>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Id))
                {
                    return ServiceResponse<object>.Error("Your id is invalid!");
                }

                var user = await _dbcontext.Users
                    .Where(x => x.Id == Convert.ToInt32(request.Id))
                    .FirstOrDefaultAsync(cancellationToken);

                if (user is null)
                {
                    return ServiceResponse<object>.NotFound("We could not find any user!");
                }

                _dbcontext.Users.Remove(user);
                await _dbcontext.SaveChangesAsync(cancellationToken);

                return ServiceResponse<object>.Success("Removing user is successfull!", null);
            }
            catch (DbUpdateException ex)
            {
                throw new AppException("خطای دیتابیس در حذف کاربر");
            }
            catch (Exception ex)
            {
                throw new AppException("حذف کاربر ناموفق بود");
            }
        }

        public class Input
        {
            private string Id { get; set; }
            private string? Name { get; set; }

            public Input(string id)
            {
                this.Id = id;
            }

            public Input(string id, string name)
            {
                this.Id = id;
                this.Name = name;
            }
        }
    }
}
