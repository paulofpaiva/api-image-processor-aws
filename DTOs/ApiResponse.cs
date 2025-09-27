namespace api_image_processor_aws.DTOs;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public object Data { get; set; } = new { };

    public static ApiResponse<T> Success(object? data, string message = "")
        => new ApiResponse<T> { IsSuccess = true, Message = message,  Data = data ?? new { } };

    public static ApiResponse<T> Fail(string message)
        => new ApiResponse<T> { IsSuccess = false, Message = message};
}