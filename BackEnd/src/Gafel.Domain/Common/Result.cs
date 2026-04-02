namespace Gafel.Domain.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? ErrorMessage { get; }

    private Result(T? value, bool isSuccess, string? errorMessage = null)
    {
        Value = value;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static Result<T> Success(T value) => new(value: value, isSuccess: true);
    public static Result<T> Failure(string errorMessage) => new(value: default, isSuccess: false, errorMessage: errorMessage);
}
