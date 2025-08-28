using Serilog;
namespace Infrastructure.Logging
{
    public class LoggerService : ILoggerService
    {
        public void LogError(string message, Exception ex)
        {
            Log.Error(message, ex);
        }

        public void LogInfo(string message)
        {
            Log.Information(message);
        }
    }
}
