using StayNest.Application.Abstractions.Authentication;
using StayNest.Application.Abstractions.PageSize;
using StayNest.Application.Listings.Responses;
using StayNest.Domain.Reservations.Errors;
using StayNest.Domain.Reservations.Interface;
using StayNest.SharedKernel.ErrorHandling;
using StayNest.SharedKernel.Helpers;
using StayNest.SharedKernel.Messaging.Query;

namespace StayNest.Application.Listings.Queries.GetReservations;

public sealed class GetReservationsQueryHandler
    : IQueryHandler<GetReservationsQuery, PaginatedResult<ReservationResponse>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUserContext _userContext;

    public GetReservationsQueryHandler(
        IReservationRepository reservationRepository,
        IUserContext userContext)
    {
        _reservationRepository = reservationRepository;
        _userContext = userContext;
    }

    public async Task<Result<PaginatedResult<ReservationResponse>>> Handle(
        GetReservationsQuery request,
        CancellationToken cancellationToken)
    {
        var query = request.Query ?? new QueryObject();

        var userId = _userContext.UserId;

        var reservations = await _reservationRepository.GetAllAsync(
            query,
            userId,
            cancellationToken);

        var mapped = reservations
            .Select(ReservationResponse.FromEntity)
            .ToList();

        var totalCount = await _reservationRepository.CountAsync(userId, cancellationToken);

        var result = new PaginatedResult<ReservationResponse>(
            mapped,
            totalCount,
            query.Page,
            query.PageSize);

        return Result.Success(result);
    }
}