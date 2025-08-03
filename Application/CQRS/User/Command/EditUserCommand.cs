//using Application.DTO;
//using MediatR;
//using Microsoft.EntityFrameworkCore;

//namespace Application.CQRS.User
//{
//    public class EditUserCommand : IRequest<string>
//    {
//        public EditUserDto editUser { get; set; }
//    }
//    public class EditUserCommandHandler : IRequestHandler<EditUserCommand, string>
//    {
//        private readonly ProgramDbContext _dbContext;

//        public EditUserCommandHandler(ProgramDbContext programDbContext)
//        {
//            _dbContext = programDbContext;
//        }

//        public async Task<string> Handle(EditUserCommand request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                var data = request.editUser;
//                if (!int.TryParse(data.Id, out int result))
//                {
//                    throw new Exception("Id is not correct!");
//                }

//                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == result);
//                if (user is null)
//                    throw new Exception("We could not find any user!");

//                user.Username = data.Username;
//                user.Password = data.Password;
//                await _dbContext.SaveChangesAsync();
//                return "Edit is successfull!";
//            }
//            catch (Exception ex)
//            {
//                return "Edit is Faild!";
//            }

//        }
//    }
//}
