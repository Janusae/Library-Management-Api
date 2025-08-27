using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class Password_Management : IPasswordManagement
    {
        private readonly PasswordHasher<object> _hasher = new();
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty or whitespace.", nameof(password));
            return _hasher.HashPassword(null , password);
        }
        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var result = _hasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
