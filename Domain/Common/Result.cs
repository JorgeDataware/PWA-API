namespace PWA_API.Domain.Common;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T? Value { get; private set; }
    public string? Error { get; private set; }
    public int StatusCode { get; private set; }

    private Result(bool isSuccess, T? value, string? error, int statusCode)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        StatusCode = statusCode;
    }

    public static Result<T> Success(T value, int statusCode = 200) =>
        new(true, value, null, statusCode);

    public static Result<T> Failure(string error, int statusCode = 400) =>
        new(false, default, error, statusCode);

    public static Result<T> NotFound(string error = "Resource not found") =>
        new(false, default, error, 404);

    public static Result<T> Unauthorized(string error = "Unauthorized") =>
        new(false, default, error, 401);

    public static Result<T> Forbidden(string error = "Forbidden") =>
        new(false, default, error, 403);
}
