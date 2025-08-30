using Application.Common;
using Application.DTO;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class LoginCommand : IRequest<ServiceResponse<string>>
    {
        public LoginDto LoginDto { get; set; }
    }
    public class LoginHandler : IRequestHandler<LoginCommand, ServiceResponse<string>>
    {
        private readonly ProgramDbContext _dbContext;
        private readonly IPasswordManagement _passwordManagement;

        public LoginHandler(ProgramDbContext dbContext, IPasswordManagement passwordManagement)
        {
            _dbContext = dbContext;
            _passwordManagement = passwordManagement;
        }

        public async Task<ServiceResponse<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var instance = request.LoginDto;
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.IsDeleted == false && instance.Username == x.Username);
            var comparePass = _passwordManagement.VerifyPassword(user.PasswordHash, instance.Password);
            if (user != null && comparePass)
            {
                var claims = new[]
                {
                        new Claim(ClaimTypes.Name , instance.Username),
                        new Claim(ClaimTypes.NameIdentifier, user.NationalCode)
                    };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("c5f0e609953d1e8721005d3b275e85c6cc387f8f74883ec152cd06c5cc3e8029"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: "Hospital",
                    audience: "Admin",
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                    );
                var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                return ServiceResponse<string>.Success("Success", jwt);
            }
            else
            {
                return ServiceResponse<string>.Error("User not found");

            }
        }
    }
}
