using Application.DTO;
using Infrastructure.Context;
using MediatR;
using Application.Services;
namespace Application.CQRS.User
{
    public class CreateUserCommand : IRequest<string>
    {
        public CreateUserDto createUser { get; set; }
    }
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly ProgramDbContext _dbContext;

        public CreateUserHandler(ProgramDbContext programDbContext)
        {
            _dbContext = programDbContext;
        }

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var data = request.createUser;
                var passHash = new Password_Management();
                var user = new Domain.Sql.Entity.User
                {
                    CreatedAt = DateTime.Now,
                    Email = data.Email,
                    FirstName = data.Firstname,
                    LastName = data.Lastname,
                    Username = data.Username,
                    PasswordHash = passHash.HashPassword(data.Password)
                };
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return "Success";
            }
            catch(Exception ex)
            {
                return "Faild";
            }
            
        }
    }
}
