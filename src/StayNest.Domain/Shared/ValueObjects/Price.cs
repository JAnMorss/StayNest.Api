using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Shared.ValueObjects;

public sealed class Price : ValueObject
{
    private Price(decimal amount)
    {
        Amount = amount;
    }

    public decimal Amount { get; }

    public static Result<Price> Create(decimal amount)
    {
        if (amount <= 0)
        {
            return Result.Failure<Price>(new Error(
                "Price.Invalid",
                "Price must be greater than zero."
            ));
        }

        return Result.Success(new Price(amount));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Amount;
    }
}
