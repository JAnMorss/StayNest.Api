using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Listings.ValueObjects;

public sealed class BathroomCount : ValueObject
{
    public const int MaxValue = 50;

    private BathroomCount(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<BathroomCount> Create(int bathroomCount)
    {
        if (bathroomCount < 0)
        {
            return Result.Failure<BathroomCount>(new Error(
                "BathroomCount.Invalid",
                "Bathroom count cannot be negative."));
        }

        if (bathroomCount > MaxValue)
        {
            return Result.Failure<BathroomCount>(new Error(
                "BathroomCount.TooLong",
                $"Bathroom count cannot exceed {MaxValue}."));
        }

        return Result.Success(new BathroomCount(bathroomCount));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
