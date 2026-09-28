using StayNest.Application.Abstractions.Authentication;
using StayNest.Application.Listings.Responses;
using StayNest.Domain.Listings.Interface;
using StayNest.SharedKernel.ErrorHandling;
using StayNest.SharedKernel.Messaging.Query;

namespace StayNest.Application.Listings.Queries.GetUserFavorites;

public sealed class GetUserFavoritesQueryHandler
    : IQueryHandler<GetUserFavoritesQuery, IList<FavoriteResponse>>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;

    public GetUserFavoritesQueryHandler(
        IListingRepository listingRepository,
        IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _userContext = userContext;
    }

    public async Task<Result<IList<FavoriteResponse>>> Handle(
        GetUserFavoritesQuery request,
        CancellationToken cancellationToken)
    {
        var favorites = await _listingRepository.GetUserFavoritesAsync(
            _userContext.UserId,
            cancellationToken);

        var response = favorites
            .Select(FavoriteResponse.FromEntity)
            .ToList();

        return Result.Success<IList<FavoriteResponse>>(response);
    }
}