using Application.DTO.BookDto;
using Application.Exceptions;
using Application.Validations;
using Application.Validations.Book;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Book.Command
{
    public class CreateBookCommand : IRequest<string>
    {
        public CreateBookDto CreateBookDto { get; set; }
    }
    public class CreateBookHandler : IRequestHandler<CreateBookCommand, string>
    {
        private readonly ProgramDbContext _programDb;
        private static SemaphoreSlim Semaphore = new SemaphoreSlim(1 , 1);
        public CreateBookHandler(ProgramDbContext programDb)
        {
            _programDb = programDb;
        }
        public async Task<string> Handle (CreateBookCommand request , CancellationToken cancellationToken)
        {
            try
            {
                await Semaphore.WaitAsync(cancellationToken);

                var instance = request.CreateBookDto;
                var validator = new CreateBookDtoValidator();
                var validatorResult = validator.Validate(instance);
                if (!validatorResult.IsValid)
                    return $"{validatorResult.Errors[0]}";

                var checkRepeatable = await _programDb.Book.AnyAsync(x => x.Name == instance.Name , cancellationToken);
                if (checkRepeatable is true)
                    return "The name of book is already exist!";

                var book = new Domain.Sql.Entity.Book()
                {
                    Description = instance.Description,
                    IsDeleted = false,
                    IsExist = instance.IsExist ?? true,
                    Name = instance.Name
                };
                await _programDb.Book.AddAsync(book, cancellationToken);
                await _programDb.SaveChangesAsync(cancellationToken);
                return "کتاب با موفقیت ایجاد شد";
            }
            catch (DbUpdateException ex) 
            {
                throw new AppException("خطای دیتابیس در ذخیره کتاب" , "500");
            }
            catch (Exception ex)
            {
                throw new AppException("ایجاد کتاب ناموفق بود", "500");
            }
            finally
            {
                Semaphore.Release();
            }

        } 
    }
}
