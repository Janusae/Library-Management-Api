using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class LoginCommand : IRequest<string>
    {
        public string Fullname { get; set; }
        public string Nationalcode { get; set; }
    }
    public class LoginHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly ProgramDbContext _dbContext;

        public LoginHandler(ProgramDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.AnyAsync(x => x.IsDeleted == false);
            if (user is not false)
            {
                var claims = new[]
                {
                        new Claim(ClaimTypes.Name , request.Fullname),
                        new Claim(ClaimTypes. NameIdentifier, request.Nationalcode)
                    };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsASecretKeyThatIsLongEnoughToBeSecure123!!"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: "Hospital",
                    audience: "Admin",
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                    );
                var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                return jwt;
            }
            else
            {
                return ("User not found");
            }
        }
    }
}
