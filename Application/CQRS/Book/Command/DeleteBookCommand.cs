using MediatR;
using Application.DTO.BookDto;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Application.Exceptions;

namespace Application.CQRS.Book.Command
{
    public class DeleteBookCommand : IRequest<string>
    {
        public string Id { get; set; }
    }
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, string>
    {
        private readonly ProgramDbContext _programDb;
        public DeleteBookCommandHandler(ProgramDbContext programDb)
        {
            _programDb = programDb;
        }
        public async Task<string> Handle (DeleteBookCommand request , CancellationToken cancellationToken)
        {
            try
            {
                var id = Convert.ToInt32(request.Id);
                var book = await _programDb.Book.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                _programDb.Book.Remove(book);
                await _programDb.SaveChangesAsync(cancellationToken);
                return "کتاب با موفقیت حذف شد";
            }
            catch (DbUpdateException ex) 
            {
                throw new AppException("خطای دیتابیس در حذف کتاب" , "500");
            }
            catch (Exception ex)
            {
                throw new AppException("حذف کتاب ناموفق بود", "500");
            }

        } 
    }
}
