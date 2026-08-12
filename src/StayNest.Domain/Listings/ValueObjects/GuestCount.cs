using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Listings.ValueObjects;

public sealed class GuestCount : ValueObject
{
    public const int MaxValue = 100;

    private GuestCount(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<GuestCount> Create(int guestCount)
    {
        if (guestCount <= 0)
        {
            return Result.Failure<GuestCount>(new Error(
                "GuestCount.Invalid",
                "Guest count must be greater than zero."
            ));
        }

        if (guestCount > MaxValue)
        {
            return Result.Failure<GuestCount>(new Error(
                "GuestCount.TooLong",
                $"Guest count cannot exceed {MaxValue}."
            ));
        }

        return Result.Success(new GuestCount(guestCount));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
