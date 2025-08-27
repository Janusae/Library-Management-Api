namespace Domain.Sql.Entity
{
    public class User : Base
    {
        public string? FirstName { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string NationalCode { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}
