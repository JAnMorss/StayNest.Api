using StayNest.Domain.Favorites.Entities;
using StayNest.Domain.Listings.Entities;
using StayNest.SharedKernel.Repositories;

namespace StayNest.Domain.Listings.Interface;

public interface IListingRepository : IRepository<Listing>
{
    Task AddFavoriteAsync(Guid userId, Guid listingId, CancellationToken cancellationToken = default);

    Task RemoveFavoriteAsync(Guid userId, Guid listingId, CancellationToken cancellationToken = default);

    Task<bool> IsFavoritedAsync(Guid userId, Guid listingId, CancellationToken cancellationToken = default);

    Task<IList<Favorite>> GetUserFavoritesAsync(Guid userId, CancellationToken cancellationToken = default);
}
