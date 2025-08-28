using Infrastructure.Logging;

namespace Application.Common
{
    public class ResponseHandler
    {
        private readonly ILoggerService _logger;

        public ResponseHandler(ILoggerService logger)
        {
            _logger = logger;
        }

        public ServiceResponse<T> CreateSuccess<T>(string message, T data)
        {
            _logger.LogInfo(message);
            return ServiceResponse<T>.Success(message, data);
        }

        public ServiceResponse<T> CreateError<T>(string message, Exception? ex = null)
        {
            _logger.LogError(message, ex);
            return ServiceResponse<T>.Error(message);
        }

        public ServiceResponse<T> CreateNotFound<T>(string message)
        {
            _logger.LogInfo($"NotFound: {message}");
            return ServiceResponse<T>.NotFound(message);
        }
    }
}
