using Infrastructure.Context;
using MediatR;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Application.Exceptions;
using Application.Validations;
using Application.DTO.User;
namespace Application.CQRS.User
{
    public class CreateUserCommand : IRequest<string>
    {
        public CreateUserDto createUser { get; set; }
    }
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, string>
    {
        private static ProgramDbContext _dbContext;
        private readonly IPasswordManagement _passwordManagement;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public CreateUserHandler(ProgramDbContext programDbContext, IPasswordManagement passwordManagement)
        {
            _dbContext = programDbContext;
            _passwordManagement = passwordManagement;
        }

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                var validator = new CreateUserDtoValidator();
                var validationResult = validator.Validate(request.createUser);
                if (!validationResult.IsValid)
                    return $"{validationResult.Errors[0]}";

                var data = request.createUser;
                if (CheckDouplicate(data) is false)
                    return "Email or Username is exist!";

                var user = new Domain.Sql.Entity.User
                {
                    CreatedAt = DateTime.Now,
                    Email = data.Email,
                    FirstName = data.Firstname,
                    LastName = data.Lastname,
                    Username = data.Username,
                    PasswordHash = _passwordManagement.HashPassword(data.Password),
                    IsDeleted = false
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
            finally
            {
                _semaphore.Release();
            }
            
        }
        public static bool CheckDouplicate(CreateUserDto data)
        {
            var result = _dbContext.Users.Any(x => x.Email == data.Email || x.Username == data.Username);
            if (result is true)
                return false;
            return true;
        }
    }
}
