using Application.DTO;
using Infrastructure.Context;
using MediatR;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Application.Exceptions;
using Application.Validations;
namespace Application.CQRS.User
{
    public class CreateUserCommand : IRequest<string>
    {
        public CreateUserDto createUser { get; set; }
    }
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly ProgramDbContext _dbContext;
        private readonly IPasswordManagement _passwordManagement;
        public CreateUserHandler(ProgramDbContext programDbContext, IPasswordManagement passwordManagement)
        {
            _dbContext = programDbContext;
            _passwordManagement = passwordManagement;
        }

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreateUserDtoValidator();
                var validationResult = validator.Validate(request.createUser);
                if (!validationResult.IsValid)
                    return $"{validationResult.Errors[0]}";

                var data = request.createUser;
                var user = new Domain.Sql.Entity.User
                {
                    CreatedAt = DateTime.Now,
                    Email = data.Email,
                    FirstName = data.Firstname,
                    LastName = data.Lastname,
                    Username = data.Username,
                    PasswordHash = _passwordManagement.HashPassword(data.Password)
                };
                await _dbContext.Users.AddAsync(user , cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return "Creating user is successfull!";
            }
            catch(DbUpdateException ex)
            {
                throw new AppException("خطای دیتابیس در ذخیره کاربر", "500");
            }            catch(Exception ex)

            {
                throw new AppException("ایجاد کاربر ناموفق بود" , "500");
            }
            
        }
    }
}
