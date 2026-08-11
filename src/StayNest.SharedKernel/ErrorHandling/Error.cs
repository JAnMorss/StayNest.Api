namespace StayNest.SharedKernel.ErrorHandling;

public record Error(string Code, string Message)
{
    public static readonly Error None = new(
        string.Empty,
        string.Empty
    );

    public static readonly Error NullValue = new(
        "Error.NullValue",
        "A null value was encountered where it is not allowed."
    );

    public static implicit operator Result(Error error)
        => Result.Failure(error);

    public Result ToResult()
        => Result.Failure(this);
}
