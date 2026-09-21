using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Users.ValueObjects;

public sealed class FirstName : ValueObject
{
    public const int MaxLength = 50;

    private FirstName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<FirstName> Create(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<FirstName>(new Error(
                "FirstName.Empty",
                "FirstName cannot be empty."));
        }

        if (firstName.Length > MaxLength)
        {
            return Result.Failure<FirstName>(new Error(
                "FirstName.TooLong",
                $"Firstname is too long. Maximum Length is {MaxLength} characters."));
        }

        return Result.Success(new FirstName(firstName));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

}