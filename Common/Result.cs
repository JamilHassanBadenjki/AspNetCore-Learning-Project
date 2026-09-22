namespace FirstApi.Common;

public class Result<T>
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public T? Value { get; }

    public Error? Error { get; }

    private Result(
        bool isSuccess,
        T? value,
        Error? error)
    {
        if (isSuccess && value is null)
        {
            throw new InvalidOperationException(
                "A successful result must contain a value.");
        }

        if (isSuccess && error is not null)
        {
            throw new InvalidOperationException(
                "A successful result cannot contain an error.");
        }

        if (!isSuccess && error is null)
        {
            throw new InvalidOperationException(
                "A failed result must contain an error.");
        }

        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(
            true,
            value,
            null);
    }

    public static Result<T> Failure(Error error)
    {
        return new Result<T>(
            false,
            default,
            error);
    }
}

public class Result
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error? Error { get; }

    private Result(
        bool isSuccess,
        Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success()
    {
        return new Result(
            true,
            null);
    }

    public static Result Failure(Error error)
    {
        return new Result(
            false,
            error);
    }
}