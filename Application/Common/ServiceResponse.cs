namespace Application.Common
{
    public enum StatusResponse
    {
        Success = 200,
        Error = 400,
        NotFound = 404
    }

    public class ServiceResponse<T>
    {
        public string? Message { get; set; }
        public StatusResponse StatusResponse { get; set; }
        public T? Data { get; set; }

        public static ServiceResponse<T> Success(string message , T data)
        {
            return new ServiceResponse<T>
            {
                Message = message,
                StatusResponse = StatusResponse.Success,
                Data = data
            };
        }

        public static ServiceResponse<T> Error(string message)
        {
            return new ServiceResponse<T>
            {
                Message = message,
                StatusResponse = StatusResponse.Error,
                Data = default
            };
        }

        public static ServiceResponse<T> NotFound(string message)
        {
            return new ServiceResponse<T>
            {
                Message = message,
                StatusResponse = StatusResponse.NotFound,
                Data = default
            };
        }
    }
}
