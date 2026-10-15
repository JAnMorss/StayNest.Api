using StayNest.Application.Abstractions.Authentication;
using StayNest.Domain.Listings.Entities;
using StayNest.Domain.Listings.Errors;
using StayNest.Domain.Reservations.Errors;
using StayNest.Domain.Reservations.Interface;
using StayNest.Domain.Shared;
using StayNest.SharedKernel.ErrorHandling;
using StayNest.SharedKernel.Messaging.Command;

namespace StayNest.Application.Listings.Commands.CancelReservation;

public sealed class CancelReservationCommandHandler
    : ICommandHandler<CancelReservationCommand>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelReservationCommandHandler(
        IReservationRepository reservationRepository,
        IUnitOfWork unitOfWork)
    {
        _reservationRepository = reservationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        CancelReservationCommand request, 
        CancellationToken cancellationToken)
    {
        var reservation = await _reservationRepository.GetByIdAsync(request.ReservationId, cancellationToken);
        if (reservation is null)
            return Result.Failure(ReservationErrors.NotFound);

        await _reservationRepository.DeleteAsync(reservation.Id, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
