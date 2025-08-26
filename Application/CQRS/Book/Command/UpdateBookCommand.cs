using Application.Common;
using Application.DTO.BookDto;
using Application.Exceptions;
using Application.Validations.Book;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Application.CQRS.Book.Command
{
    public class UpdateBookCommand : IRequest<ServiceResponse<object>>
    {
        public UpdateBookDto updateBookDto { get; set; }
    }

    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, ServiceResponse<object>>
    {
        private readonly ProgramDbContext _programDb;
        private static SemaphoreSlim semaphor = new SemaphoreSlim(1, 1);

        public UpdateBookCommandHandler(ProgramDbContext programDb)
        {
            _programDb = programDb;
        }

        public async Task<ServiceResponse<object>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            await semaphor.WaitAsync();
            try
            {
                var instance = request.updateBookDto;
                var validator = new updateBookDtoValidator();
                var validatorResult = validator.Validate(instance);

                if (!validatorResult.IsValid)
                    return ServiceResponse<object>.Error($"{validatorResult.Errors[0]}");

                var id = Convert.ToInt32(instance.Id);
                var book = await _programDb.Book.FirstOrDefaultAsync(x => x.Id == id);

                if (book is null)
                    return ServiceResponse<object>.NotFound("We could not find any book with your id!");

                var checkRepeatalbe = await _programDb.Book.AnyAsync(x => x.Name == instance.Name);
                if (checkRepeatalbe)
                    return ServiceResponse<object>.Error("The name of book is already exist!");

                book.IsExist = instance.IsExist;
                book.Name = instance.Name;
                book.Description = instance.Description;

                await _programDb.SaveChangesAsync(cancellationToken);

                return ServiceResponse<object>.Success("کتاب با موفقیت آپدیت شد", null);
            }
            catch (DbUpdateException ex)
            {
                throw new AppException("خطای دیتابیس در آپدیت کتاب", "500");
            }
            catch (Exception ex)
            {
                throw new AppException("آپدیت کتاب ناموفق بود", "500");
            }
            finally
            {
                semaphor.Release();
            }
        }
    }
}
