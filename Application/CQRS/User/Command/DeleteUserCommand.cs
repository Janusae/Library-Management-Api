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
        private readonly ResponseHandler _response;

        public DeleteUserHandler(ProgramDbContext dbcontext, ResponseHandler response)
        {
            _dbcontext = dbcontext;
            _response = response;
        }

        public async Task<ServiceResponse<object>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Id))
                {
                    return _response.CreateError<object>("Your id is invalid!");
                }

                var user = await _dbcontext.Users
                    .Where(x => x.Id == Convert.ToInt32(request.Id))
                    .FirstOrDefaultAsync(cancellationToken);

                if (user is null)
                {
                    return _response.CreateNotFound<object>("We could not find any user!");
                }

                _dbcontext.Users.Remove(user);
                await _dbcontext.SaveChangesAsync(cancellationToken);

                return _response.CreateSuccess<object>("Removing user is successfull!", null);
            }
            catch (DbUpdateException ex)
            {
                _response.CreateError<object>("خطای دیتابیس در حذف کاربر", ex);
                throw new AppException("خطای دیتابیس در حذف کاربر");
            }
            catch (Exception ex)
            {
                _response.CreateError<object>("حذف کاربر ناموفق بود", ex);
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
