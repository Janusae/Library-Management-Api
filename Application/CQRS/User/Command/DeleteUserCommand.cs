//namespace Application.CQRS.User
//{
//    public class DeleteUserCommand : IRequest<string>
//    {
//        public string Id { get; set; }
//    }
//    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, string>
//    {
//        private readonly ProgramDbContext _dbcontext;

//        public DeleteUserHandler(ProgramDbContext dbcontext)
//        {
//            _dbcontext = dbcontext;
//        }

//        public async Task<string> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                var user = _dbcontext.Users.FirstOrDefault(x => x.Id == Convert.ToInt32(request.Id));
//                _dbcontext.Users.Remove(user);
//                await _dbcontext.SaveChangesAsync();
//                return "Successfull";
//            }
//            catch(Exception ex)
//            {
//                return "Failed";
//            }
            
//        }
//        public class Input
//        {
//            private string Id { get; set; }
//            private string? Name { get; set; }
//            public Input(string id)
//            {
//                this.Id = id;
//            }
//            public Input(string id , string name)
//            {
//                this.Id = id;
//                this.Name = name;
//            }
//        }
//    }
//}
