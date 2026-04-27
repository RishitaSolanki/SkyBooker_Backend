namespace SkyBooker.FlightService.Common;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public int StatusCode { get; set; }
    public List<string> Errors { get; set; } = new();

    public static ApiResponse<T> Success(T data, string message = "Success", int statusCode = 200)
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data,
            StatusCode = statusCode
        };
    }

    public static ApiResponse<T> Failure(string message, int statusCode = 400, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Message = message,
            StatusCode = statusCode,
            Errors = errors ?? new List<string>()
        };
    }

    public static ApiResponse<T> Failure(List<string> errors, int statusCode = 400)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Message = "Validation failed",
            StatusCode = statusCode,
            Errors = errors
        };
    }
}