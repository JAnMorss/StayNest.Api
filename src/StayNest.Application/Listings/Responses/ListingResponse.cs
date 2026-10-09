using StayNest.Domain.Listings.Entities;

namespace StayNest.Application.Listings.Responses;

public sealed class ListingResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string CreatedBy { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Category { get; set; } = null!;

    public int RoomCount { get; set; }
    public int BathroomCount { get; set; }
    public int GuestCount { get; set; }

    public string Location { get; set; } = null!;
    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<string> ImageUrls { get; set; } = new();

    public List<FavoriteResponse> Favorites { get; set; } = new();

    public List<ReservationResponse> Reservations { get; set; } = new();

    public static ListingResponse FromEntity(Listing listing)
    {
        return new ListingResponse
        {
            Id = listing.Id,
            UserId = listing.UserId,

            CreatedBy = $"{listing.User.FirstName.Value} {listing.User.LastName.Value}".Trim(),

            Title = listing.Title?.Value ?? string.Empty,
            Description = listing.Description?.Value,
            Category = listing.Category?.Value ?? string.Empty,

            RoomCount = listing.RoomCount?.Value ?? 0,
            BathroomCount = listing.BathroomCount?.Value ?? 0,
            GuestCount = listing.GuestCount?.Value ?? 0,

            Location = listing.LocationValue?.Value ?? string.Empty,
            Price = listing.Price?.Amount ?? 0,

            CreatedAt = listing.CreatedAt,
            UpdatedAt = listing.UpdatedAt,

            ImageUrls = listing.ImageSrc
                .Select(image => image.Value)
                .ToList(),

            Favorites = listing.Favorites
                .Select(f => FavoriteResponse.FromEntity(f))
                .ToList(),

            Reservations = listing.Reservations
                .Select(r => ReservationResponse.FromEntity(r))
                .ToList()
        };
    }
}
