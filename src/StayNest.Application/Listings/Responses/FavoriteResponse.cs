using StayNest.Domain.Favorites.Entities;

namespace StayNest.Application.Listings.Responses;

public sealed class FavoriteResponse
{
    public Guid UserId { get; set; }
    public Guid ListingId { get; set; }
    public string? ListingTitle { get; set; }

    public static FavoriteResponse FromEntity(Favorite favorite)
    {
        return new FavoriteResponse
        {
            UserId = favorite.UserId,
            ListingId = favorite.ListingId,
            ListingTitle = favorite.Listing.Title.Value
        };
    }
}