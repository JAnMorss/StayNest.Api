using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Listings.ValueObjects;

public sealed class Description : ValueObject
{
    public const int MaxLength = 1000;

    private Description(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Description> Create(string description)
    {
        if(string.IsNullOrWhiteSpace(description))
        {
            return Result.Failure<Description>(new Error(
                "Description.Empty",
                "Description cannot be empty."
            ));
        }

        if(description.Length > MaxLength)
        {
            return Result.Failure<Description>(new Error(
                "Description.TooLong",
                $"Description cannot exceed {MaxLength} characters."
            ));
        }

        return Result.Success(new Description(description));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
