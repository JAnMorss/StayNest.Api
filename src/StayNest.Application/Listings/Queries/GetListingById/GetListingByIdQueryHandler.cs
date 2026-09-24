using StayNest.Application.Listings.Responses;
using StayNest.Domain.Listings.Errors;
using StayNest.Domain.Listings.Interface;
using StayNest.SharedKernel.ErrorHandling;
using StayNest.SharedKernel.Messaging.Query;

namespace StayNest.Application.Listings.Queries.GetListingById;

public sealed class GetListingByIdQueryHandler
    : IQueryHandler<GetListingByIdQuery, ListingResponse>
{
    private readonly IListingRepository _listingRepository;

    public GetListingByIdQueryHandler(IListingRepository listingRepository)
    {
        _listingRepository = listingRepository;
    }

    public async Task<Result<ListingResponse>> Handle(
        GetListingByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing is null)
            return Result.Failure<ListingResponse>(ListingErrors.NotFound);

        var response = ListingResponse.FromEntity(listing);

        return Result.Success(response);
    }
}
