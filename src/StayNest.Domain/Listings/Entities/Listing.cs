using StayNest.Domain.Listings.ValueObjects;
using StayNest.Domain.Users.Entities;
using StayNest.SharedKernel.Domain;

namespace StayNest.Domain.Listings.Entities;

public sealed class Listing : BaseEntity
{
    private Listing() { }

    public Listing(
        Guid id,
        Title title,
        string description,
        string imageSrc,
        string category,
        int roomCount,
        int bathroomCount,
        int guestCount,
        string locationValue,
        int price,
        Guid userId
        ) : base(id)
    {
        Title = title;
        Description = description;
        ImageSrc = imageSrc;
        CreatedAt = DateTime.UtcNow;
        Category = category;
        RoomCount = roomCount;
        BathroomCount = bathroomCount;
        GuestCount = guestCount;
        LocationValue = locationValue;
        Price = price;
        UserId = userId;
    }

    public Title Title { get; private set; }
    public string Description { get; private set; }
    public string ImageSrc { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string Category { get; private set; }
    public int RoomCount { get; private set; }
    public int BathroomCount { get; private set; }
    public int GuestCount { get; private set; }
    public string LocationValue { get; private set; }
    public int Price { get; private set; }

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
}
