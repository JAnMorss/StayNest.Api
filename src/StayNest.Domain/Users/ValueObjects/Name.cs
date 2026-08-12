using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Users.ValueObjects;

public sealed class Name : ValueObject
{
    public const int MaxLength = 100;

    private Name(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Name> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Name>(new Error(
                "Name.Empty",
                "Name cannot be empty."
            ));
        }

        if (name.Length > MaxLength)
        {
            return Result.Failure<Name>(new Error(
                "Name.TooLong",
                $"Name is too long. Maximum Length is {MaxLength} character."
            ));
        }

        return Result.Success(new Name(name));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value; 
    }
}
