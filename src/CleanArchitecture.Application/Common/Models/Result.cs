namespace CleanArchitecture.Application.Common.Models;

public sealed record Result<T>(bool Succeeded, T? Data, string? Error)
{
    public static Result<T> Success(T data) => new(true, data, null);
    public static Result<T> Failure(string error) => new(false, default, error);
}
