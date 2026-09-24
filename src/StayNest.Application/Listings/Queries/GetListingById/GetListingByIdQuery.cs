using StayNest.Application.Listings.Responses;
using StayNest.SharedKernel.Messaging.Query;

namespace StayNest.Application.Listings.Queries.GetListingById;

public sealed record GetListingByIdQuery(Guid ListingId) : IQuery<ListingResponse>;