namespace CleanArchitecture.Api.Contracts;

public sealed record ApiResponse<T>(bool Success, int Code, string Message, T? Data)
{
    public static ApiResponse<T> Ok(T? data, int code, string message) => new(true, code, message, data);
    public static ApiResponse<T> Fail(int code, string message, T? data = default) => new(false, code, message, data);
}
