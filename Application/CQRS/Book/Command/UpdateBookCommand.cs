using MediatR;
using Application.DTO.BookDto;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Application.Exceptions;

namespace Application.CQRS.Book.Command
{
    public class UpdateBookCommand : IRequest<string>
    {
        public UpdateBookDto updateBookDto { get; set; }
    }
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, string>
    {
        private readonly ProgramDbContext _programDb;
        public UpdateBookCommandHandler(ProgramDbContext programDb)
        {
            _programDb = programDb;
        }
        public async Task<string> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var instance = request.updateBookDto;
                var id = Convert.ToInt32(instance.Id);
                var book = await _programDb.Book.FirstOrDefaultAsync(x => x.Id == id);
                book.IsExist = instance.IsExist;
                book.Name = instance.Name;
                book.Description = instance.Description;
                await _programDb.SaveChangesAsync(cancellationToken);
                return "کتاب با موفقیت آپدیت شد";
            }
            catch (DbUpdateException ex)
            {
                throw new AppException("خطای دیتابیس در آپدیت کتاب", "500");
            }
            catch (Exception ex)
            {
                throw new AppException("آپدیت کتاب ناموفق بود", "500");
            }

        }
    }
}
