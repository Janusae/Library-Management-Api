namespace Infrastructure.Logging
{
    public interface ILoggerService
    {
        void LogInfo(string message);
        void LogError(string message , Exception ex);
    }
}
