using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class Password_Management
    {
        private readonly PasswordHasher<object> _hasher = new();
        public string HashPassword(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Password cannot be empty or whitespace.", nameof(text));
            return _hasher.HashPassword(null , text);

        }
    }
}
