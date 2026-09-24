using StayNest.Application.Abstractions.Authentication;
using StayNest.Application.Abstractions.PageSize;
using StayNest.Application.Listings.Responses;
using StayNest.Domain.Listings.Errors;
using StayNest.Domain.Listings.Interface;
using StayNest.SharedKernel.ErrorHandling;
using StayNest.SharedKernel.Helpers;
using StayNest.SharedKernel.Messaging.Query;

namespace StayNest.Application.Listings.Queries.GetListing;

public sealed class GetListingsQueryHandler
    : IQueryHandler<GetListingsQuery, PaginatedResult<ListingResponse>>
{
    private readonly IListingRepository _listingRepository;

    public GetListingsQueryHandler(IListingRepository listingRepository)
    {
        _listingRepository = listingRepository;
    }

    public async Task<Result<PaginatedResult<ListingResponse>>> Handle(
        GetListingsQuery request, 
        CancellationToken cancellationToken)
    {
        var query = request.Query ?? new QueryObject();

        var listing = await _listingRepository.GetAllAsync(query, cancellationToken);
        if (listing is null)
            return Result.Failure<PaginatedResult<ListingResponse>>(ListingErrors.NotFound);

        var mapped = listing
            .Select(ListingResponse.FromEntity)
            .ToList();

        var totalCount = await _listingRepository.CountAsync(cancellationToken);

        var result = new PaginatedResult<ListingResponse>(
            mapped,
            totalCount,
            query.Page,
            query.PageSize);

        return Result.Success(result);
    }
}
