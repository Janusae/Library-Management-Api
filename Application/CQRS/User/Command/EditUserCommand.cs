using Application.DTO;
using Application.Services;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.User
{
    public class EditUserCommand : IRequest<string>
    {
        public EditUserDto editUser { get; set; }
    }
    public class EditUserCommandHandler : IRequestHandler<EditUserCommand, string>
    {
        private readonly ProgramDbContext _dbContext;

        public EditUserCommandHandler(ProgramDbContext programDbContext)
        {
            _dbContext = programDbContext;
        }

        public async Task<string> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var data = request.editUser;
                var passHash = new Password_Management();
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(data.Id));
                if (user is null)
                    throw new Exception("We could not find any user!");

                user.Username = data.Username;
                user.PasswordHash = passHash.HashPassword(data.Password);
                await _dbContext.SaveChangesAsync();
                return "Edit is successfull!";
            }
            catch (Exception ex)
            {
                return "Edit is Faild!";
            }

        }
    }
}
