using Application.Exceptions;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.CQRS.User
{
    public class DeleteUserCommand : IRequest<string>
    {
        public string Id { get; set; }
    }
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, string>
    {
        private readonly ProgramDbContext _dbcontext;

        public DeleteUserHandler(ProgramDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<string> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Id) || !int.TryParse(request.Id, out int id))
                {
                    return "Your id is invalid!";
                }

                var user = await _dbcontext.Users.Where(x => x.Id == Guid.Parse(request.Id)).FirstOrDefaultAsync();
                _dbcontext.Users.Remove(user );
                await _dbcontext.SaveChangesAsync(cancellationToken);
                return "Removing user is successfull!";
            }
            catch(DbUpdateException ex)
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
