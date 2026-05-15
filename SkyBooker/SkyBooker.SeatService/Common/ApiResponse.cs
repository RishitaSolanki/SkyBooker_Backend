namespace SkyBooker.SeatService.Common;

public class ApiResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public List<string> Errors { get; set; } = new List<string>();

    public static ApiResponse Success(string message, int statusCode = 200)
    {
        return new ApiResponse { IsSuccess = true, Message = message, StatusCode = statusCode };
    }

    public static ApiResponse Failure(string message, int statusCode = 400)
    {
        return new ApiResponse { IsSuccess = false, Message = message, StatusCode = statusCode };
    }
}

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new List<string>();

    public static ApiResponse<T> Success(T data, string message = "Success", int statusCode = 200)
    {
        return new ApiResponse<T> { IsSuccess = true, Message = message, Data = data, StatusCode = statusCode };
    }

    public static ApiResponse<T> Failure(string message, int statusCode = 400)
    {
        return new ApiResponse<T> { IsSuccess = false, Message = message, StatusCode = statusCode };
    }
}
