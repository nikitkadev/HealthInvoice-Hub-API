namespace HealthInvoice.Core.Common;

public class Result(
    bool isSuccess, 
    string errorMessage, 
    Guid errorId)
{
    public bool IsSuccess { get; } = isSuccess;
    public string ErrorMessage { get; } = errorMessage;
    public Guid ErrorId { get; } = errorId;

    public bool IsFailure => !IsSuccess;

    public static Result Success() => new(true, string.Empty, Guid.Empty);
    public static Result Fail(string errorMessage, Guid errorId) => new(false, errorMessage, errorId);
}

public class Result<T>(
    T? value, 
    bool isSuccess, 
    string errorMessage, 
    Guid errorId) : Result(isSuccess, errorMessage, errorId)
{
    public T? Value { get; } = value;

    public static Result<T> Success(T value) => new(value, true, string.Empty, Guid.Empty);
    public static new Result<T> Fail(string errorMessage, Guid errorId) => new(default, false, errorMessage, errorId);
}
