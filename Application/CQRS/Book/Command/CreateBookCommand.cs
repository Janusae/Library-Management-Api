using Application.DTO.BookDto;
using Application.Exceptions;
using Application.Validations.Book;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common;

namespace Application.CQRS.Book.Command
{
    public class CreateBookCommand : IRequest<ServiceResponse<object>>
    {
        public CreateBookDto CreateBookDto { get; set; }
    }

    public class CreateBookHandler : IRequestHandler<CreateBookCommand, ServiceResponse<object>>
    {
        private readonly ProgramDbContext _programDb;
        private readonly ResponseHandler _response;
        private static SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);

        public CreateBookHandler(ProgramDbContext programDb, ResponseHandler response)
        {
            _programDb = programDb;
            _response = response;
        }

        public async Task<ServiceResponse<object>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            await Semaphore.WaitAsync(cancellationToken);
            try
            {
                var instance = request.CreateBookDto;
                var validator = new CreateBookDtoValidator();
                var validatorResult = validator.Validate(instance);

                if (!validatorResult.IsValid)
                    return _response.CreateError<object>($"{validatorResult.Errors[0]}");

                var checkRepeatable = await _programDb.Book.AnyAsync(x => x.Name == instance.Name, cancellationToken);
                if (checkRepeatable)
                    return _response.CreateError<object>("The name of book is already exist!");

                var book = new Domain.Sql.Entity.Book
                {
                    Description = instance.Description,
                    IsDeleted = false,
                    IsExist = instance.IsExist ?? true,
                    Name = instance.Name
                };

                await _programDb.Book.AddAsync(book, cancellationToken);
                await _programDb.SaveChangesAsync(cancellationToken);

                return _response.CreateSuccess<object>("کتاب با موفقیت ایجاد شد", null);
            }
            catch (DbUpdateException ex)
            {
                _response.CreateError<object>("خطای دیتابیس در ذخیره کتاب", ex);
                throw new AppException("خطای دیتابیس در ذخیره کتاب", "500");
            }
            catch (Exception ex)
            {
                _response.CreateError<object>("ایجاد کتاب ناموفق بود", ex);
                throw new AppException("ایجاد کتاب ناموفق بود", "500");
            }
            finally
            {
                Semaphore.Release();
            }
        }
    }
}
