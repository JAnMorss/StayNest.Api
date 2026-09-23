using StayNest.Application.Listings.Responses;
using StayNest.SharedKernel.Messaging.Query;

namespace StayNest.Application.Listings.Queries.GetListingImage;

public sealed record GetListingImageQuery(Guid ListingId) 
    : IQuery<ListingImageResponse>;