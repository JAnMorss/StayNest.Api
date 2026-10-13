using StayNest.Application.Abstractions.PageSize;
using StayNest.Application.Listings.Responses;
using StayNest.SharedKernel.Helpers;
using StayNest.SharedKernel.Messaging.Query;

namespace StayNest.Application.Listings.Queries.GetReservations;

public sealed record GetReservationsQuery(QueryObject? Query) 
    : IQuery<PaginatedResult<ReservationResponse>>;
