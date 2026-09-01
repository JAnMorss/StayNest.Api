using StayNest.Domain.Listings.ValueObjects;
using StayNest.Domain.Reservations.Entities;
using StayNest.Domain.Shared.ValueObjects;
using StayNest.Domain.Users.Entities;
using StayNest.SharedKernel.Domain;

namespace StayNest.Domain.Listings.Entities;

public sealed class Listing : BaseEntity
{
    private readonly List<Reservation> _reservations = new();

    private Listing() { }
    public Listing(
        Guid id,
        Guid userId,
        Title title,
        Description description,
        Image imageSrc,
        Category category,
        RoomCount roomCount,
        BathroomCount bathroomCount,
        GuestCount guestCount,
        LocationValue locationValue,
        Price price
    ) : base(id)
    {
        UserId = userId;
        Title = title;
        Description = description;
        ImageSrc = imageSrc;
        Category = category;
        RoomCount = roomCount;
        BathroomCount = bathroomCount;
        GuestCount = guestCount;
        LocationValue = locationValue;
        Price = price;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid UserId { get; private set; }
    public Title Title { get; private set; } = null!;
    public Description Description { get; private set; } = null!;
    public Image ImageSrc { get; private set; } = null!;
    public Category Category { get; private set; } = null!;
    public RoomCount RoomCount { get; private set; } = null!;
    public BathroomCount BathroomCount { get; private set; } = null!;
    public GuestCount GuestCount { get; private set; } = null!;
    public LocationValue LocationValue { get; private set; } = null!;
    public Price Price { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }

    public User User { get; private set; } = null!;

    public IReadOnlyCollection<Reservation> Reservations
        => _reservations.AsReadOnly();
}
