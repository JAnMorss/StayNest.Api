using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Listings.ValueObjects;

public sealed class LocationValue : ValueObject
{
    public const int MaxLength = 200;

    private LocationValue(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<LocationValue> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<LocationValue>(new Error(
                "LocationValue.Empty",
                "Location value cannot be empty."
            ));
        }

        if (value.Length > MaxLength)
        {
            return Result.Failure<LocationValue>(new Error(
                "LocationValue.TooLong",
                $"Location value cannot exceed {MaxLength} characters."
            ));
        }

        return Result.Success(new LocationValue(value));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
