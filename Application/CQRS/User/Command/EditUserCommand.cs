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
        private static ProgramDbContext _dbContext;
        private readonly IPasswordManagement _passwordManagement;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public EditUserCommandHandler(ProgramDbContext programDbContext, IPasswordManagement passwordManagement)
        {
            _dbContext = programDbContext;
            _passwordManagement = passwordManagement;
        }

        public async Task<string> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                var validator = new EditUserDtoValidator();
                var validationResult = validator.Validate(request.editUser);
                if (!validationResult.IsValid)
                    return $"{validationResult.Errors[0]}";

                var data = request.editUser;
                if (CheckDouplicate(data) is false)
                    return "Username is exist!";
                if (!int.TryParse(data.Id, out int result))
                    return "Id is invalid!";
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(result));
                if (user is null)
                    return "We could not find any user!";

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
            finally
            {
                _semaphore.Release();
            }
        }
        private static bool CheckDouplicate(EditUserDto data)
        {
            var result = _dbContext.Users.Any(x => x.Username == data.Username);
            if (result is true)
                return false;
            return true;
        }
    }
}
