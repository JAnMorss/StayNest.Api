using StayNest.Application.Abstractions.Authentication;
using StayNest.Application.Listings.Responses;
using StayNest.Domain.Listings.Errors;
using StayNest.Domain.Listings.Interface;
using StayNest.SharedKernel.ErrorHandling;
using StayNest.SharedKernel.Messaging.Query;

namespace StayNest.Application.Listings.Queries.GetListingImage;

public sealed class GetListingImageQueryHandler
    : IQueryHandler<GetListingImageQuery, ListingImageResponse>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;

    public GetListingImageQueryHandler(
        IListingRepository listingRepository,
        IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _userContext = userContext;
    }

    public async Task<Result<ListingImageResponse>> Handle(
        GetListingImageQuery request, 
        CancellationToken cancellationToken)
    {
        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing is null)
            return Result.Failure<ListingImageResponse>(ListingErrors.NotFound);

        if (listing.UserId != _userContext.UserId)
            return Result.Failure<ListingImageResponse>(ListingErrors.NotFound);

        if (!listing.ImageSrc.Any())
            return Result.Failure<ListingImageResponse>(ListingErrors.ImageNotFound);

        var response = ListingImageResponse.FromEntity(listing);

        return Result.Success(response);
    }
}
