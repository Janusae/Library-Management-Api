using Application.DTO;
using Application.Exceptions;
using Application.Services;
using Application.Validations;
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
        private readonly IPasswordManagement _passwordManagement;
        public EditUserCommandHandler(ProgramDbContext programDbContext, IPasswordManagement passwordManagement)
        {
            _dbContext = programDbContext;
            _passwordManagement = passwordManagement;
        }

        public async Task<string> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new EditUserDtoValidator();
                var validationResult = validator.Validate(request.editUser);
                if (!validationResult.IsValid)
                    return $"{validationResult.Errors[0]}";

                var data = request.editUser;
                var repeatableUsername = _dbContext.Users.Any(x => x.Username == request.editUser.Username);
                if (repeatableUsername)
                {
                    return "Username is used by anther person!";
                }

                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(data.Id));
                if (user is null)
                    throw new Exception("We could not find any user!");

                user.Username = data.Username;
                user.PasswordHash = _passwordManagement.HashPassword(data.Password);
                await _dbContext.SaveChangesAsync();
                return "Edit is successfull!";
            }
            catch(DbUpdateException ex)
            {
                throw new AppException("خطای دیتابیس در ادیت کاربر" , "500");
            }
            catch (Exception ex)
            {
                throw new AppException("ادیت کاربر ناموفق بود" , "500");
            }

        }
    }
}
