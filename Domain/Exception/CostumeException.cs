namespace Domain.Exceptions
{
    // تعریف کلاس استثنا سفارشی که از Exception ارث می‌برد
    public class CustomException : Exception
    {
        public int StatusCode { get; }  // خاصیت برای نگهداری کد وضعیت

        // سازنده ساده که پیام خطا را به کلاس پایه ارسال می‌کند
        public CustomException(string message) : base(message)
        {
        }

        public CustomException(string message, int statusCode, Exception innerException)
                   : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
