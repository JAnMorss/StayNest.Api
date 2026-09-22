using StayNest.Domain.Listings.Errors;
using StayNest.Domain.Listings.Events;
using StayNest.Domain.Listings.ValueObjects;
using StayNest.Domain.Reservations.Entities;
using StayNest.Domain.Shared.ValueObjects;
using StayNest.Domain.Users.Entities;
using StayNest.SharedKernel.Domain;
using StayNest.SharedKernel.ErrorHandling;

namespace StayNest.Domain.Listings.Entities;

public sealed class Listing : BaseEntity
{
    private readonly List<Reservation> _reservations = new();
    private readonly List<Image> _images = new();

    private Listing() { }
    public Listing(
        Guid id,
        Guid userId,
        Title title,
        Description description,
        Category category,
        RoomCount roomCount,
        BathroomCount bathroomCount,
        GuestCount guestCount,
        LocationValue locationValue,
        Price price) : base(id)
    {
        UserId = userId;
        Title = title;
        Description = description;
        Category = category;
        RoomCount = roomCount;
        BathroomCount = bathroomCount;
        GuestCount = guestCount;
        LocationValue = locationValue;
        Price = price;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public Guid UserId { get; private set; }
    public Title Title { get; private set; } = null!;
    public Description Description { get; private set; } = null!;
    public Category Category { get; private set; } = null!;
    public RoomCount RoomCount { get; private set; } = null!;
    public BathroomCount BathroomCount { get; private set; } = null!;
    public GuestCount GuestCount { get; private set; } = null!;
    public LocationValue LocationValue { get; private set; } = null!;
    public Price Price { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public User User { get; private set; } = null!;

    public IReadOnlyCollection<Reservation> Reservations
        => _reservations.AsReadOnly();

    public IReadOnlyCollection<Image> ImageSrc
        => _images.AsReadOnly();

    public static Result<Listing> Create(
        Guid userId,
        Title title,
        Description description,
        Category category,
        RoomCount roomCount,
        BathroomCount bathroomCount,
        GuestCount guestCount,
        LocationValue locationValue,
        Price price)
    {
        var listing = new Listing(
            Guid.NewGuid(),
            userId,
            title,
            description,
            category,
            roomCount,
            bathroomCount,
            guestCount,
            locationValue,
            price);

        listing.RaiseDomainEvent(new ListingCreatedDomainEvent(listing.Id));

        return Result.Success(listing);
    }

    public Result Update(
        Title title,
        Description description,
        Category category,
        RoomCount roomCount,
        BathroomCount bathroomCount,
        GuestCount guestCount,
        LocationValue locationValue,
        Price price)
    {

        Title = title;
        Description = description;
        Category = category;
        RoomCount = roomCount;
        BathroomCount = bathroomCount;
        GuestCount = guestCount;
        LocationValue = locationValue;
        Price = price;

        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result AddImages(IEnumerable<string> imageUrls)
    {
        foreach (var url in imageUrls)
        {
            var result = Image.Create(url);
            if (result.IsFailure)
                return Result.Failure(result.Error);

            _images.Add(result.Value);
        }

        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result RemoveImages(string url)
    {
        var media = _images.FirstOrDefault(x => x.Value == url);
        if (media is null)
            return Result.Failure(ListingErrors.ListingNotFound);

        _images.Remove(media);

        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }
}