using System.Runtime.InteropServices.JavaScript;

namespace AgendaPro.Domain.Shared;

public class Result<TValue>
{
    public TValue? Value;
    public Error Error;
    public bool IsSuccess;
    public bool IsFailure => !IsSuccess;

    private Result(TValue value)
    {
        IsSuccess = true;
        Value = value;
        Error = Error.None;
    }
    private Result(Error error)
    {
        IsSuccess = false;
        Value = default;
        Error = error;
    }

    public static Result<TValue> Success(TValue value) => new(value);
    public static Result<TValue> Failure(Error error) => new(error);
}