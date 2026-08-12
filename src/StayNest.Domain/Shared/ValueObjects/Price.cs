using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Shared.ValueObjects;

public sealed class Price : ValueObject
{
    private Price(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }

    public static Result<Price> Create(decimal price)
    {
        if (price <= 0)
        {
            return Result.Failure<Price>(new Error(
                "Price.Invalid",
                "Price must be greater than zero."
            ));
        }

        return Result.Success(new Price(price));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
