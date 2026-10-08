using StayNest.Application.Abstractions.Authentication;
using StayNest.Domain.Listings.Errors;
using StayNest.Domain.Listings.Interface;
using StayNest.Domain.Reservations.Entities;
using StayNest.Domain.Reservations.Interface;
using StayNest.Domain.Reservations.Services;
using StayNest.Domain.Shared;
using StayNest.Domain.Shared.ValueObjects;
using StayNest.SharedKernel.ErrorHandling;
using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Listings.Commands.Reserve;

public sealed class ReserveCommandHandler
    : ICommandHandler<ReserveCommand>
{
    private readonly IListingRepository _listingRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly PricingService _pricingService;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public ReserveCommandHandler(
        IListingRepository listingRepository, 
        IReservationRepository reservationRepository, 
        PricingService pricingService, 
        IUserContext userContext, 
        IUnitOfWork unitOfWork)
    {
        _listingRepository = listingRepository;
        _reservationRepository = reservationRepository;
        _pricingService = pricingService;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        ReserveCommand request, 
        CancellationToken cancellationToken)
    {
        var periodResult = DateRange.Create(request.StartDate, request.EndDate);
        if (periodResult.IsFailure)
            return Result.Failure(periodResult.Error);

        var period = periodResult.Value;

        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing is null)
            return Result.Failure(ListingErrors.NotFound);

        var userId = _userContext.UserId;

        if (listing.UserId == userId)
            return Result.Failure(ListingErrors.CannotReserveOwnListing);

        if (!listing.IsAvailable(period))
            return Result.Failure(ListingErrors.ListingNotAvailable);

        var pricingResult = _pricingService.CalculatePrice(listing, period);
        if (pricingResult.IsFailure)
            return Result.Failure(pricingResult.Error);

        var reservationResult = Reservation.Create(
            listing.Id,
            userId,
            period,
            pricingResult.Value);

        if (reservationResult.IsFailure)
            return Result.Failure(reservationResult.Error);

        var reservation = reservationResult.Value;

        var addReservationResult = listing.AddReservation(reservation);
        if (addReservationResult.IsFailure)
            return Result.Failure(addReservationResult.Error);

        await _reservationRepository.AddAsync(reservation, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
