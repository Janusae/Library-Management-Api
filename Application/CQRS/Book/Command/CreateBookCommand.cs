using MediatR;
using Application.DTO.BookDto;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Application.Exceptions;

namespace Application.CQRS.Book.Command
{
    public class CreateBookCommand : IRequest<string>
    {
        public CreateBookDto CreateBookDto { get; set; }
    }
    public class CreateBookHandler : IRequestHandler<CreateBookCommand, string>
    {
        private readonly ProgramDbContext _programDb;
        public CreateBookHandler(ProgramDbContext programDb)
        {
            _programDb = programDb;
        }
        public async Task<string> Handle (CreateBookCommand request , CancellationToken cancellationToken)
        {
            try
            {
                var instance = request.CreateBookDto;
                var book = new Domain.Sql.Entity.Book()
                {
                    Description = instance.Description,
                    IsDeleted = false,
                    IsExist = instance.IsExist,
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

        } 
    }
}
