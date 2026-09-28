using StayNest.Application.Listings.Responses;
using StayNest.SharedKernel.Messaging.Query;

namespace StayNest.Application.Listings.Queries.GetUserFavorites;

public sealed record GetUserFavoritesQuery() : IQuery<IList<FavoriteResponse>>;
