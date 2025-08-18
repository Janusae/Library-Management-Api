namespace Application.Exceptions
{
    public class AppException : Exception
    {
        public string StatusCode { get; set; }
        public AppException() : base()
        {
            
        }
        public AppException(string message) : base(message) 
        {
            
        }
        public AppException(string message, string statusCode) : base(message) 
        {
            StatusCode = statusCode;
        }
    }
}
