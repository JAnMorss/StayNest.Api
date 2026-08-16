using StayNest.Domain.Listings.Entities;
using StayNest.Domain.Users.Entities;

namespace StayNest.Domain.Favorites.Entities;

public sealed class Favorite
{
    private Favorite() { }
    public Favorite(Guid userId, Guid listingId)
    {
        UserId = userId;
        ListingId = listingId;
    }

    public Guid UserId { get; private set; }
    public Guid ListingId { get; private set; }
    public User User { get; private set; } = null!;
    public Listing Listing { get; private set; } = null!;


}
