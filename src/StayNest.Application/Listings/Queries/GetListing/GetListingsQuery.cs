
using StayNest.Application.Abstractions.PageSize;
using StayNest.Application.Listings.Responses;
using StayNest.SharedKernel.Helpers;
using StayNest.SharedKernel.Messaging.Query;

namespace StayNest.Application.Listings.Queries.GetListing;

public sealed record GetListingsQuery(QueryObject? Query)
    : IQuery<PaginatedResult<ListingResponse>>;
