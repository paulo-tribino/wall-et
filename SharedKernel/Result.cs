namespace SharedKernel;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result<T> Success<T>(T value) =>
       new(true, Error.None, value);

    public static Result Failure(Error error) => new(false, error);

    public static Result<T> Failure<T>(Error error) =>
        new(false, error);
}

public sealed class Result<T> : Result
{
    internal Result(bool isSuccess, Error error, T? value = default)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public T? Value { get; }
}