using StayNest.Domain.Listings.Entities;
using StayNest.Domain.Reservations.Enums;
using StayNest.Domain.Reservations.Errors;
using StayNest.Domain.Shared.ValueObjects;
using StayNest.Domain.Users.Entities;
using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Reservations.Entities;

public sealed class Reservation : BaseEntity
{
    private Reservation() { }

    public Reservation(
        Guid id,
        Guid userId,
        Guid listingId,
        DateRange period,
        Price totalPrice) : base(id)
    {
        UserId = userId;
        ListingId = listingId;
        Period = period;
        TotalPrice = totalPrice;

        Status = ReservationStatus.Confirmed;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public Guid UserId { get; private set; }
    public Guid ListingId { get; private set; }
    public DateRange Period { get; private set; } = null!;
    public Price TotalPrice { get; private set; }
    public ReservationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public User User { get; private set; } = null!;
    public Listing Listing { get; private set; } = null!;

    public static Result<Reservation> Create(
        Guid userId,
        Guid listingId,
        DateRange period,
        Price totalprice)
    {
        var reservation = new Reservation(
            Guid.NewGuid(),
            userId,
            listingId,
            period,
            totalprice);

        return Result.Success(reservation);
    }

    public Result Cancel()
    {
        if (Status.Equals(ReservationStatus.Cancelled))
            return Result.Failure(ReservationErrors.AlreadyCancelled);

        Status = ReservationStatus.Cancelled;

        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

}
