namespace Application.Services
{
    public interface IPasswordManagement
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string providedPassword);
    }
}
