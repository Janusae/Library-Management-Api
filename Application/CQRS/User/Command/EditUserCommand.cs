using Application.Common;
using Application.DTO.User;
using Application.Exceptions;
using Application.Services;
using Application.Validations;
using Application.Validations.User;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.User
{
    public class EditUserCommand : IRequest<ServiceResponse<object>>
    {
        public EditUserDto editUser { get; set; }
    }

    public class EditUserCommandHandler : IRequestHandler<EditUserCommand, ServiceResponse<object>>
    {
        private static ProgramDbContext _dbContext;
        private readonly IPasswordManagement _passwordManagement;
        private readonly ResponseHandler _response;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public EditUserCommandHandler(ProgramDbContext programDbContext, IPasswordManagement passwordManagement, ResponseHandler response)
        {
            _dbContext = programDbContext;
            _passwordManagement = passwordManagement;
            _response = response;
        }

        public async Task<ServiceResponse<object>> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);

            try
            {
                var validator = new EditUserDtoValidator();
                var validationResult = validator.Validate(request.editUser);

                if (!validationResult.IsValid)
                    return _response.CreateError<object>($"{validationResult.Errors[0]}");

                var data = request.editUser;

                if (!CheckDouplicate(data))
                    return _response.CreateError<object>("Username is exist!");

                if (!int.TryParse(data.Id, out int result))
                    return _response.CreateError<object>("Id is invalid!");

                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == result);
                if (user is null)
                    return _response.CreateNotFound<object>("We could not find any user!");

                user.Username = data.Username;
                user.PasswordHash = _passwordManagement.HashPassword(data.Password);

                await _dbContext.SaveChangesAsync();

                return _response.CreateSuccess<object>("Edit is successfull!", null);
            }
            catch (DbUpdateException ex)
            {
                _response.CreateError<object>("خطای دیتابیس در ادیت کاربر", ex);
                throw new AppException("خطای دیتابیس در ادیت کاربر", "500");
            }
            catch (Exception ex)
            {
                _response.CreateError<object>("ادیت کاربر ناموفق بود", ex);
                throw new AppException("ادیت کاربر ناموفق بود", "500");
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private static bool CheckDouplicate(EditUserDto data)
        {
            return !_dbContext.Users.Any(x => x.Username == data.Username);
        }
    }
}
