//using Domain.SQL.Token;
//using Infrastructure.Context;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace Application.Services
//{
//    public class LogoutCommand : IRequest<string>
//    {
//        public string Token { get; set; }
//    }
//    public class LogoutHandler : IRequestHandler<LogoutCommand, string>
//    {
//        private readonly ProgramDbContext _dbContext;

//        public LogoutHandler(ProgramDbContext dbContext)
//        {
//            _dbContext = dbContext;
//        }

//        public async Task<string> Handle(LogoutCommand request, CancellationToken cancellationToken)
//        {
//            //    var handler = new JwtSecurityTokenHandler();
//            //    var jwtToken = handler.ReadJwtToken(request.Token);
//            //    var expiry = jwtToken.ValidTo;
//            //    await _dbContext.BlackListToken.AddAsync(new Domain.SQL.Token.BlackListToken
//            //    {
//            //        Token = request.Token,
//            //        Expiration = expiry
//            //    });
//            //    await _dbContext.SaveChangesAsync();
//            return "True";
//        }
//    }
//}
