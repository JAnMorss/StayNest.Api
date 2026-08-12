using StayNest.Domain.Listings.Entities;
using StayNest.Domain.Shared.ValueObjects;
using StayNest.Domain.Users.Entities;
using StayNest.SharedKernel.Domain;

namespace StayNest.Domain.Reservations.Entities;

public sealed class Reservation : BaseEntity
{
    private Reservation() { }

    public Reservation(
        Guid id,
        Guid userId,
        Guid listingId,
        DateTime startDate, 
        DateTime endDate,
        Price totalPrice
    ) : base(id)
    {
        UserId = userId;
        ListingId = listingId;
        StartDate = startDate;
        EndDate = endDate;
        TotalPrice = totalPrice;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid UserId { get; private set; }
    public Guid ListingId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public Price TotalPrice { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User User { get; private set; } = null!;
    public Listing Listing { get; private set; } = null!;
}
