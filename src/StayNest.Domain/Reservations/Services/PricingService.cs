using StayNest.Domain.Listings.Entities;
using StayNest.Domain.Shared.ValueObjects;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Reservations.Services;

public sealed class PricingService
{
    public Result<Price> CalculatePrice(
        Listing listing,
        DateRange period
    )
    {
        var amount = listing.Price.Amount * period.LengthInDays;

        return Price.Create(amount);
    }
}
