using StayNest.Application.Abstractions.Authentication;
using StayNest.Application.Listings.Responses;
using StayNest.Domain.Listings.Errors;
using StayNest.Domain.Listings.Interface;
using StayNest.SharedKernel.ErrorHandling;
using StayNest.SharedKernel.Messaging.Query;

namespace StayNest.Application.Listings.Queries.GetMyListingById;

public sealed class GetMyListingByIdQueryHandler
    : IQueryHandler<GetMyListingByIdQuery, ListingResponse>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;

    public GetMyListingByIdQueryHandler(
        IListingRepository listingRepository,
        IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _userContext = userContext;
    }

    public async Task<Result<ListingResponse>> Handle(
        GetMyListingByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing is null)
            return Result.Failure<ListingResponse>(ListingErrors.NotFound);

        if (listing.UserId != _userContext.UserId)
            return Result.Failure<ListingResponse>(ListingErrors.NotFound);

        var response = ListingResponse.FromEntity(listing);

        return Result.Success(response);
    }
}
