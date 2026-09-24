using StayNest.Application.Listings.Responses;
using StayNest.SharedKernel.Messaging.Query;

namespace StayNest.Application.Listings.Queries.GetMyListingById;

public sealed record GetMyListingByIdQuery(Guid ListingId) : IQuery<ListingResponse>;