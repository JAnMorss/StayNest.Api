using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Listings.ValueObjects;

public sealed class RoomCount : ValueObject
{
    public const int MaxValue = 100;

    private RoomCount(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<RoomCount> Create(int roomCount)
    {
        if(roomCount <= 0)
        {
            return Result.Failure<RoomCount>(new Error(
                "RoomCount.Invalid",
                "Room count must be greater than zero."
            ));
        }

        if (roomCount > MaxValue)
        {
            return Result.Failure<RoomCount>(new Error(
                "RoomCount.TooLong",
                $"Room count cannot exceed {MaxValue}."
            ));
        }

        return Result.Success(new RoomCount(roomCount));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
