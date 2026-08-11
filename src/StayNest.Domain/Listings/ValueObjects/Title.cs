using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Listings.ValueObjects;

public sealed class Title : ValueObject
{
    public const int MaxLength = 100;

    public Title(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Title> Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result.Failure<Title>(new Error(
                "Title.Empty",
                "Title cannot be empty."
            ));
        }

        if (title.Length > MaxLength)
        {
            return Result.Failure<Title>(new Error(
                "Title.TooLong",
                $"Title cannot exceed {MaxLength} characters"
            )); 
        }

        return Result.Success(new Title(title));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
