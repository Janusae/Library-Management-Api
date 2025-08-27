using Application.Common;
using Application.DTO.User;
using Application.Exceptions;
using Application.Services;
using Application.Validations.User;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.User
{
    public class CreateUserCommand : IRequest<ServiceResponse<object>>
    {
        public CreateUserDto createUser { get; set; }
    }

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, ServiceResponse<object>>
    {
        private static ProgramDbContext _dbContext;
        private readonly IPasswordManagement _passwordManagement;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public CreateUserHandler(ProgramDbContext programDbContext, IPasswordManagement passwordManagement)
        {
            _dbContext = programDbContext;
            _passwordManagement = passwordManagement;
        }

        public async Task<ServiceResponse<object>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);
            try
            {
                var validator = new CreateUserDtoValidator();
                var validationResult = validator.Validate(request.createUser);

                if (!validationResult.IsValid)
                    return ServiceResponse<object>.Error($"{validationResult.Errors[0]}");

                var data = request.createUser;

                if (!CheckDouplicate(data))
                    return ServiceResponse<object>.NotFound("Email or Username is exist");

                var user = new Domain.Sql.Entity.User
                {
                    CreatedAt = DateTime.Now,
                    Email = data.Email,
                    FirstName = data.Firstname,
                    LastName = data.Lastname,
                    NationalCode = data.NationalCode,
                    Username = data.Username,
                    PasswordHash = _passwordManagement.HashPassword(data.Password),
                    IsDeleted = false
                };

                await _dbContext.Users.AddAsync(user, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return ServiceResponse<object>.Success("Creating user is successfull", null);
            }
            catch (DbUpdateException ex)
            {
                throw new AppException("خطای دیتابیس در ذخیره کاربر", "500");
            }
            catch (Exception ex)
            {
                throw new AppException("ایجاد کاربر ناموفق بود", "500");
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public static bool CheckDouplicate(CreateUserDto data)
        {
            return !_dbContext.Users.Any(x => x.Email == data.Email || x.Username == data.Username);
        }
    }
}
