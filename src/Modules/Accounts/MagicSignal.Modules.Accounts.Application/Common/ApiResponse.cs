namespace MagicSignal.Modules.Accounts.Application.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Constructor برای Success Response
        public static ApiResponse<T> SuccessResponse(T data, string message = "عملیات با موفقیت انجام شد")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Timestamp = DateTime.UtcNow
            };
        }

        // Constructor برای Error Response
        public static ApiResponse<T> ErrorResponse(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors,
                Timestamp = DateTime.UtcNow
            };
        }

        // Constructor برای Error Response ساده
        public static ApiResponse<T> ErrorResponse(string message, string error)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = new List<string> { error },
                Timestamp = DateTime.UtcNow
            };
        }
    }

    // ورژن بدون Generic برای پیام‌های ساده
    public class ApiResponse : ApiResponse<object>
    {
        public static ApiResponse SuccessResponse(string message = "عملیات با موفقیت انجام شد")
        {
            return new ApiResponse
            {
                Success = true,
                Message = message,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}