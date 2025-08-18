namespace Application.Services
{
    public interface IPasswordManagement
    {
        string HashPassword(string password);
    }
}
